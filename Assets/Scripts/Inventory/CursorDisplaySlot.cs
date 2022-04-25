using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FactoryGame.Placements
{
    [RequireComponent(typeof(DisplayItemSlot))]
    public class CursorDisplaySlot : MonoBehaviour
    {
        public static CursorDisplaySlot Instance { get; private set; }
        public DisplayItemSlot DisplaySlot { get; private set; }

        void Awake()
        {
            Instance = this;

            DisplaySlot = GetComponent<DisplayItemSlot>();
            DisplaySlot.ItemSlotChanged += ItemSlotChanged;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void ItemSlotChanged(Item? arg1, int arg2)
        {
            // Disable gameobject if no item is in the slot.
            if (arg1 is null || arg2 == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 cursorPosition = Mouse.current.position.ReadValue();
            transform.position = cursorPosition;
        }
    }
}
