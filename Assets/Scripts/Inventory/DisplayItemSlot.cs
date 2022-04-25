using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
namespace FactoryGame.Placements
{
    public class DisplayItemSlot : ItemSlotBase
    {
        public ItemSlot? ItemSlot;
        public Image IconImage;
        public TextMeshProUGUI ItemCountText;
        public int recipeAmount { get; set; } = 0;
        public void SetItemSlot(ItemSlot? itemSlot)
        {
            // Unassign previous listener
            if (!(ItemSlot is null))
            {
                ItemSlot.ItemSlotChanged -= OnItemSlotChanged;
            }

            // Assign new listener
            if (!(itemSlot is null))
            {
                itemSlot.ItemSlotChanged += OnItemSlotChanged;
            }

            OnItemSlotChanged(itemSlot?.Item, itemSlot?.Count ?? 0);
            ItemSlot = itemSlot;
        }

        public void DisplaySlotClicked()
        {
            //Debug.Log($"Object: {this.name}, Contents: {ItemSlot?.Item?.Name}:{ItemSlot?.Count}");
            CursorDisplaySlot.Instance.DisplaySlot.SetItemSlot(ItemSlot);
        }

        // Start is called before the first frame update
        void Awake()
        {
            SetItemSlot(ItemSlot);
        }

        void OnDestroy()
        {
            // Cleanup listener
            if (ItemSlot is null)
            {
                return;
            }

            ItemSlot.ItemSlotChanged -= OnItemSlotChanged;
        }

        public void UpdateItemSlotAppearence() => UpdateItemSlotAppearence(ItemSlot.Item, ItemSlot.Count);

        void UpdateItemSlotAppearence(Item? itemType, int itemCount)
        {
            Item? item = itemType ?? ItemSlot?.Filter;
            float iconOpacity = 0;
            string itemCountString = string.Empty;

            // Has more than 0 items.
            if (itemCount != 0)
            {
                itemCountString = itemCount.ToString();
                iconOpacity = 1;
            }
            else if (ItemSlot?.Filter != null) // has no items, but has filter.
            {
                iconOpacity = 0.5f;
            }

            IconImage.color = new Color(1, 1, 1, iconOpacity);
            IconImage.sprite = item?.Icon;
            ItemCountText.text = recipeAmount > 0 ? $"{itemCountString}/{recipeAmount}" : itemCountString;
        }

        void OnItemSlotChanged(Item? itemType, int itemCount)
        {
            UpdateItemSlotAppearence(itemType, itemCount);

            InvokeItemSlotChanged(itemType, itemCount);
        }
    }
}
