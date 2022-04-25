using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorageView : MonoBehaviour
{
    public GameObject StorageViewPanel;
    public PlayerInput PlayerInput;
    private InputAction inventoryBtn;

    private void Awake()
    {
        inventoryBtn = PlayerInput.actions["InventoryOpenClose"];
    }
    // Update is called once per frame
    void Update()
    {
        if (inventoryBtn.triggered)
        {
            ShowStorageView();
        }
    }

    private void ShowStorageView()
    {
        StorageViewPanel.SetActive(StorageViewPanel.activeSelf ? false : true);
    }
}
