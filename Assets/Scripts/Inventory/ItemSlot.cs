using FactoryGame.Misc;
using System;
using System.Text;
using FactoryGame.Placements.Exceptions;

#nullable enable
namespace FactoryGame.Placements
{
    public class ItemSlot : ItemSlotBase, IPlacementSerializer
    {
        public const int MaxStackSize = 100;

        public int Capacity { get; set; } = MaxStackSize;
        public int IncomingCount { get; private set; }
        public int OutgoingCount { get; private set; }
        public int OccupiedCount => Count + OutgoingCount + IncomingCount;

        public int AvailableCount => Count - OutgoingCount;

        public int AvailableEmptyCapacity => Capacity - OccupiedCount;

        public Item? Filter;

        public Item? Item
        {
            get => internalItem;
            set
            {
                if (value is null)
                {
                    internalCount = 0;
                }

                InvokeItemSlotChanged(value, Count);
                internalItem = value;
            }
        }

        public int Count
        {
            get => internalCount;
            set
            {
                if (value == 0)
                {
                    internalItem = null;
                }

                InvokeItemSlotChanged(Item, value);
                internalCount = value;
            }
        }

        public bool Full => OccupiedCount >= Capacity;

        private int internalCount;
        private Item? internalItem;

        /// <summary>
        /// Returnerer om den er tom eller om item matcher filter.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SlotAvailable(Item item)
        {
            if (!(Filter is null))
            {
                if (Filter != item)
                {
                    return false;
                }
            }

            return OccupiedCount < Capacity;
        }
        
        public void SetContent(Item item, int count)
        {
            internalItem = item;
            internalCount = count;

            if (internalCount == 0 || internalItem is null)
            {
                internalCount = 0;
                internalItem = null;
            }

            InvokeItemSlotChanged(internalItem, internalCount);
        }

        /// <summary>
        /// Supports negative incremental values.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="change"></param>
        /// <exception cref="InvalidItemTypeException"></exception>
        public void IncrementCount(Item item, int change)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (Item is null)
            {
                Item = item;
            }
            else if (Item != item)
            {
                throw new InvalidItemTypeException(item, Item);
            }

            SetContent(Item, Count + change);
        }

        public void DecrementCount(int change)
        {
            if (Item is null)
            {
                throw new MissingItemTypeException();
            }

            IncrementCount(Item, -change);
        }

        public void ReserveContent(int reservedCount)
        {
            if (reservedCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reservedCount), "Has to be positive");
            }

            IncomingCount += reservedCount;
        }

        public void ClaimReservedContent(int reservedCount)
        {
            if (reservedCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reservedCount), "Has to be positve");
            }

            IncomingCount -= reservedCount;
            Count += reservedCount;
        }

        public void MarkItemForTransfer(int transferAmount)
        {
            if (transferAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferAmount), "Has to be positve");
            }

            OutgoingCount += transferAmount;
        }

        public void ItemTransforFinished(int transferAmount)
        {
            if (transferAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transferAmount), "Has to be positve");
            }

            OutgoingCount -= transferAmount;
            Count -= transferAmount;

            if (Count == 0)
            {
                Item = null;
            }
        }

        public string GetSerializedData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Item?.Name);
            sb.Append(',');
            sb.Append(Filter?.Name);
            sb.Append(',');

            if (Item is null && Filter is null)
            {
                // No more values to submit.
                return sb.ToString();
            }

            sb.Append(internalCount);
            sb.Append(',');
            sb.Append(IncomingCount);
            sb.Append(',');
            sb.Append(OutgoingCount);

            return sb.ToString();
        }

        public void SetSerializedData(string serializedData)
        {
            string[] fields = serializedData.Split(',');
            internalItem = null;
            Filter = null;

            string itemType = fields[0];
            if (!string.IsNullOrEmpty(itemType))
            {
                Item = ItemController.Instance.GetItemByName(itemType);
            }

            string filterType = fields[1];
            if (!string.IsNullOrEmpty(filterType))
            {
                Filter = ItemController.Instance.GetItemByName(itemType);
            }

            if (Item is null && Filter is null)
            {
                // No more values to fill out.
                return;
            }

            internalCount = int.Parse(fields[2]);

            IncomingCount = int.Parse(fields[3]);

            OutgoingCount = int.Parse(fields[4]);
        }
    }
}
