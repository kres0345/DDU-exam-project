using UnityEngine;
using System.Linq;
using FactoryGame.Placements;
using FactoryGame.UI;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryCrafting : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public ContainerInventory inventory;
    public Recipe[] Recipes;
    public DisplayItemSlot[] materialInputSlots;
    public Recipe SelectedRecipe;
    public TextMeshProUGUI ItemText;
    public Image CraftButton;
    public CraftingQueue CraftingQueue;
    Slider craftAmountSlider;
    InputAction escapeKeyPressed;
    int craftAmount;
    bool craftable;


    // Start is called before the first frame update
    void Awake()
    {
        escapeKeyPressed = PlayerInput.actions["MenuOpenClose"];
        craftAmountSlider = GetComponentInChildren<Slider>();
    }

    public void HideCraftingPanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowCraftingPanel(DisplayItemSlot itemSlot)
    {
        Item item = itemSlot.ItemSlot.Filter;
        if (item is null)
        {
            return;
        }

        foreach (Recipe recipe in Recipes)
        {
            if (recipe.OutputItem == item)
            {
                SelectedRecipe = recipe;
            }
        }

        for (int i = 0; i < materialInputSlots.Length; i++)
        {
            Image slotImage = materialInputSlots[i].transform.parent.GetComponent<Image>();
            slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0);
            if (i > SelectedRecipe.InputItems.Length - 1)
            {
                materialInputSlots[i].gameObject.SetActive(false);
            }
            else
            {
                materialInputSlots[i].ItemSlot.Item = SelectedRecipe.InputItems[i];
                materialInputSlots[i].gameObject.SetActive(true);
            }
        }
        gameObject.SetActive(true);
    }

    public void CraftBtnClicked()
    {
        if (!craftable)
        {
            return;
        }

        // Mark items, så de ikke ka trækkes ud af storage containers
        CraftingQueue.AddRecipe(SelectedRecipe, craftAmount);
        HideCraftingPanel();
    }

    void Update()
    {
        if (escapeKeyPressed.triggered)
        {
            HideCraftingPanel();
        }

        craftAmount = (int)craftAmountSlider.value;
        ItemText.text = $"{SelectedRecipe.OutputItem.name}: {craftAmount}";

        craftable = true;

        for (int i = 0; i < SelectedRecipe.InputItems.Length; i++)
        {
            int itemCount = SelectedRecipe.InputItemCount[i] * craftAmount;
            materialInputSlots[i].ItemSlot.Count = itemCount;
            ItemSlot[] itemSlots = inventory.ItemSlots.Where(slot => slot.Item == SelectedRecipe.InputItems[i]).ToArray();
            Image slotImage = materialInputSlots[i].transform.parent.GetComponent<Image>();

            if (itemSlots.Length < 1)
            {
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 255);
                craftable = false;
            }
            else
            {
                if (itemSlots[0].Count >= itemCount)
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 0);
                }
                else
                {
                    slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 255);
                    craftable = false;
                }
            }
        }

        if (CraftingQueue.QueueOccupied())
        {
            craftable = false;
        }

        if (craftable)
        {
            CraftButton.color = new Color32(100, 230, 90, 255);
        }
        else
        {
            CraftButton.color = new Color32(99, 99, 99, 255);
        }
    }
}
