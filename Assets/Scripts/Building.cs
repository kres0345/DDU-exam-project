using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using FactoryGame.Placements;
using FactoryGame.Placements;

#nullable enable
public class Building : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public Image ConstructionModeImage;
    public Sprite DeconstructionIcon;
    public Sprite ConstructionIcon;

    public ContainerInventory HotbarInventory;

    public Tilemap tilemap;
    public TileBase clearTile;
    public TileBase occupiedTile;
    private Placement? previewPlacement;
    private Placement? placementToRemove;

    private BuildMode mode;

    private InputAction inputDeconstructionMode;
    private InputAction inputRotate;
    private InputAction inputDeactivateModes;

    // Start is called before the first frame update
    void Awake()
    {
        inputDeconstructionMode = PlayerInput.actions["ActivateDeconstructionMode"];
        inputRotate = PlayerInput.actions["RotateBuilding"];
        inputDeactivateModes = PlayerInput.actions["DeactivateModes"];
    }

    private void Start()
    {
        CursorDisplaySlot.Instance.DisplaySlot.ItemSlotChanged += delegate (Item? item, int count)
        {
            mode = BuildMode.Nothing;
            if (previewPlacement != null)
            {
                Destroy(previewPlacement.gameObject);
            }

            if (item is null)
            {
                return;
            }

            if (count == 0)
            {
                return;
            }

            mode = BuildMode.Build;
            GameObject buildingGameObject = Instantiate(item.PlacementPrefab);
            previewPlacement = buildingGameObject.GetComponent<Placement>();
        };
    }

    enum BuildMode
    {
        Nothing,
        Build,
        Remove,
    }

    // Update is called once per frame
    void Update()
    {
        tilemap.ClearAllTiles();
        if (inputDeactivateModes.triggered)
        {
            CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(null);
            if (previewPlacement != null)
            {
                Destroy(previewPlacement.gameObject);
            }
            mode = BuildMode.Nothing;
        }

        // Colors sprites back to norml after red color from deconstruction mode
        if (placementToRemove != null)
        {
            placementToRemove.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Activate deconstruction mode
        if (inputDeconstructionMode.triggered)
        {
            CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(null);
            if (previewPlacement != null)
            {
                Destroy(previewPlacement.gameObject);
            }
            mode = BuildMode.Remove;
        }

        if (mode == BuildMode.Nothing)
        {
            ConstructionModeImage.enabled = false;
            return;
        }
        ConstructionModeImage.enabled = true;

        // Find tile from mouse position
        Vector3 mousePositionWorldSpace = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2Int tilePosition = GridCalculation.GetTilePosition(mousePositionWorldSpace);

        // Clear tiles before construction and deconstruction
        tilemap.ClearAllTiles();

        // Deconstruction mode with red colored sprites and object removal
        if (mode == BuildMode.Remove)
        {
            ConstructionModeImage.sprite = DeconstructionIcon;
            Deconstruction(tilePosition);
        }
        else
        {
            ConstructionModeImage.sprite = ConstructionIcon;
            if (inputRotate.triggered)
            {
                previewPlacement.Orientation = Placement.Rotate(previewPlacement.Orientation, 1);
                previewPlacement.TriggerPlacementRotated();
            }

            previewPlacement.UpdateSprite(true);
            previewPlacement.transform.position = tilePosition + (new Vector2(previewPlacement.BlockWidth, previewPlacement.BlockHeight) / 2);

            if (!PreviewBuildingTileOccupied(tilePosition))
            {
                // Place building at click with object from displayslot if buildable
                if (Mouse.current.leftButton.wasPressedThisFrame) /*ReadValue() != 1*/
                {
                    Construct(tilePosition);
                }
            }
        }
    }

    private bool PreviewBuildingTileOccupied(Vector2Int position)
    {
        bool occupied = false;

        for (int xOffset = 0; xOffset < previewPlacement.BlockWidth; xOffset++)
        {
            for (int yOffset = 0; yOffset < previewPlacement.BlockHeight; yOffset++)
            {
                Vector2Int currentPosition = position + new Vector2Int(xOffset, yOffset);

                if (PlacementController.GetPlacement(currentPosition) is null)
                {
                    tilemap.SetTile((Vector3Int)currentPosition, clearTile);
                }
                else
                {
                    tilemap.SetTile((Vector3Int)currentPosition, occupiedTile);
                    occupied = true;
                }
            }
        }

        return occupied;
    }

    /// <summary>
    /// Kaldt hver frame, når construction tilstand er aktiveret.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="previewBuildingPlacement"></param>
    private void Construct(Vector2Int position)
    {
        PlacementController.PlaceBuilding(previewPlacement, position);

        // Indstil næste preview bygning.
        ItemSlot? itemSlotSelectedBuilding = CursorDisplaySlot.Instance.DisplaySlot.ItemSlot;

        if (itemSlotSelectedBuilding == null)
        {
            Debug.LogWarning("No selected building in Construct method");
            CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(null);
            Destroy(this.previewPlacement.gameObject);
            return;
        }

        Item? itemSelectedBuilding = itemSlotSelectedBuilding.Item;

        previewPlacement.transform.position = position + new Vector2(previewPlacement.BlockWidth, previewPlacement.BlockHeight) / 2;
        Orientation orientation = previewPlacement.Orientation;

        if (previewPlacement.TryGetComponent(out Machine machine))
        {
            for (int i = 8; i >= (byte)orientation; i /= 2)
            {
                machine.RotateDisplaySlots();
            }
        }

        this.previewPlacement = Instantiate(itemSelectedBuilding.PlacementPrefab).GetComponent<Placement>();

        itemSlotSelectedBuilding.DecrementCount(1);

        if (itemSelectedBuilding == null)
        {
            Debug.LogWarning("No item selected in Construct method");
            CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(null);
            Destroy(this.previewPlacement);
            return;
        }

        // Rotate displayslots on machine to fit with orientation
        if (previewPlacement.TryGetComponent(out machine))
        {
            for (int i = 1; i < (byte)orientation; i *= 2)
            {
                machine.RotateDisplaySlots();
            }
        }

        previewPlacement.GetComponent<Placement>().Orientation = orientation;
    }

    /// <summary>
    /// Kaldt hver frame, når deconstruction tilstand er aktiveret.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void Deconstruction(Vector2Int position)
    {
        Placement? placement = PlacementController.GetPlacement(position);

        if (placement is null)
        {
            // ramte ikke placement.
            return;
        }

        placementToRemove = placement;

        SpriteRenderer buildingRenderer = placement.GetComponent<SpriteRenderer>();
        buildingRenderer.color = Color.red;

        if (Mouse.current.leftButton.ReadValue() != 1)
        {
            // musen har ikke klikket.
            return;
        }

        Deconstruct(placement.GridPosition, placement);
    }

    private void Deconstruct(Vector2Int position, Placement placement)
    {
        PlacementController.DeregisterPlacement(placement, position);
        //placement.PlacementRemoved.Invoke();
        placement.TriggerPlacementRemoved();

        // TODO: Storage chests skal ikke nemt kunne fjernes og man skal have bygninger tilbage i hotbaren

        ItemSlot hotbarSlot = HotbarInventory.InventorySlots.First(x => x.Item.PlacementPrefab.name == placement.GetPrefabName());
        hotbarSlot.IncrementCount(hotbarSlot.Item, 1);

        Destroy(placement.gameObject);
    }
}
