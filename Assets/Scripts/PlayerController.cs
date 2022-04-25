using FactoryGame.Placements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public DisplayItemSlot[] HotbarItemSlots;
    InputAction[] hotbarKeys;

    void Awake()
    {
        hotbarKeys = new InputAction[]
        {
            PlayerInput.actions["HotbarSelection1"],
            PlayerInput.actions["HotbarSelection2"],
            PlayerInput.actions["HotbarSelection3"],
            PlayerInput.actions["HotbarSelection4"],
            PlayerInput.actions["HotbarSelection5"],
            PlayerInput.actions["HotbarSelection6"],
            PlayerInput.actions["HotbarSelection7"],
            PlayerInput.actions["HotbarSelection8"],
            PlayerInput.actions["HotbarSelection9"],
            PlayerInput.actions["HotbarSelection0"],
            PlayerInput.actions["HotbarSelection+"]
        };
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hotbarKeys.Length; i++)
        {
            if (hotbarKeys[i].triggered)
            {
                if (CursorDisplaySlot.Instance.DisplaySlot.ItemSlot != HotbarItemSlots[i].ItemSlot)
                {
                    CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(HotbarItemSlots[i].ItemSlot);
                }
                else
                {
                    CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(null);
                }
            }
        }
    }
}
