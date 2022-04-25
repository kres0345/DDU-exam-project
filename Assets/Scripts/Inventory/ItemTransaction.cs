using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class ItemTransaction
    {
        public float ExecutionTimestamp { get; private set; }

        readonly ItemSlot outSlot;
        readonly ItemSlot inSlot;

        public ItemTransaction(ItemSlot outStack, ItemSlot inStack)
        {
            this.outSlot = outStack;
            this.inSlot = inStack;

            inSlot.Item = outSlot.Item;
        }

        public void StartTransaction(float executionTimestamp)
        {
            ExecutionTimestamp = executionTimestamp;
            outSlot.MarkItemForTransfer(1);
            inSlot.ReserveContent(1);
        }

        public void FinishTransaction()
        {
            outSlot.ItemTransforFinished(1);
            inSlot.ClaimReservedContent(1);
        }
    }
}
