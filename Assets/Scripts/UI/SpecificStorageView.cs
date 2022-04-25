using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FactoryGame.Placements;

public class SpecificStorageView : MonoBehaviour
{
    public static SpecificStorageView Instance { get; private set; }
    private InputAction closeUIView;
    public DisplayItemSlot[] InventorySlots;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowStorageView(ContainerInventory inventory)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (i < inventory.InventorySlots.Length)
            {
                InventorySlots[i].gameObject.SetActive(true);
                InventorySlots[i].SetItemSlot(inventory.InventorySlots[i]);
            }
            else
            {
                InventorySlots[i].gameObject.SetActive(false);
            }
        }
    }
}
