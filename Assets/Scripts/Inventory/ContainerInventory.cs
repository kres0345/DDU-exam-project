using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class ContainerInventory : MonoBehaviour
    {
        public ItemSlot[] InventorySlots;
        /// <summary>
        /// If true, inventory is registered with <c>InventoryController</c> on <c>Awake</c>.
        /// </summary>
        public bool RegisterInventory = false;

        public IEnumerable<ItemSlot> ItemSlots => InventorySlots;

        void Awake()
        {
            //if (RegisterInventory)
            //{
            //    InventoryController.Inventories.Add(this);
            //}
        }

        void OnDestroy()
        {
            //if (RegisterInventory)
            //{
            //    InventoryController.Inventories.Remove(this);
            //}
        }

        public int GetAvailableSpace() => GetAvailableSpace(null);

        public int GetAvailableSpace(Item? itemType)
        {
            if (itemType is null)
            {
                return ItemSlots.Sum(s => (s.Item is null && s.Filter is null) ? s.AvailableEmptyCapacity : 0);
            }
            else
            {
                return ItemSlots.Sum(s => s.SlotAvailable(itemType) ? s.AvailableEmptyCapacity : 0);
            }
        }

        public int GetItemAmount(Item itemType)
        {
            return ItemSlots.Sum(s => s.Item == itemType ? s.AvailableCount : 0);
        }

        public bool ConsumeItems(Item itemType, int count)
        {
            Queue<ItemSlot> slotsToRemoveFrom = new Queue<ItemSlot>();

            int missingSpace = count;

            foreach (var itemSlot in ItemSlots)
            {
                if (itemSlot.Item != itemType)
                {
                    continue;
                }

                if (itemSlot.AvailableCount <= 0)
                {
                    continue;
                }

                missingSpace -= itemSlot.AvailableCount;
                slotsToRemoveFrom.Enqueue(itemSlot);
            }

            if (missingSpace > 0)
            {
                return false;
            }

            missingSpace = count;

            while (slotsToRemoveFrom.Count > 0)
            {
                ItemSlot slot = slotsToRemoveFrom.Dequeue();

                int itemsToRemove = Mathf.Min(missingSpace, slot.AvailableCount);
                slot.DecrementCount(itemsToRemove);
                missingSpace -= itemsToRemove;
            }

            return true;
        }

        public bool AddItems(Item itemType, int count)
        {
            var availableItemSlots = from slot in ItemSlots
                                     where !slot.Full
                                     let restrictedItemType = slot.Item ?? slot.Filter
                                     where restrictedItemType == itemType || restrictedItemType is null
                                     // sortér tomme felter sidst
                                     orderby restrictedItemType is null
                                     select slot;

            Queue<ItemSlot> usedSlots = new Queue<ItemSlot>();

            int countedEmptySpace = 0;

            foreach (var item in availableItemSlots)
            {
                countedEmptySpace += item.AvailableEmptyCapacity;
                usedSlots.Enqueue(item);

                if (countedEmptySpace >= count)
                {
                    break;
                }
            }

            if (countedEmptySpace < count)
            {
                return false;
            }

            while (count > 0)
            {
                ItemSlot itemSlot = usedSlots.Dequeue();

                int itemsToAdd = Mathf.Min(count, itemSlot.AvailableEmptyCapacity);
                count -= itemsToAdd;

                itemSlot.IncrementCount(itemType, itemsToAdd);
            }

            return true;
        }

        public int CountItemsByType(Item itemType) => ItemSlots.Sum(s => s.Item == itemType ? s.AvailableCount : 0);
    }
}
