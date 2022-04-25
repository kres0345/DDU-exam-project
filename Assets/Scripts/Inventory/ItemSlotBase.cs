using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    public abstract class ItemSlotBase : MonoBehaviour
    {
        /// <summary>
        /// Invoked right before either item count or item type changes. Parameters are the new values.
        /// </summary>
        public event Action<Item?, int>? ItemSlotChanged;

        protected void InvokeItemSlotChanged(Item? item, int count) => ItemSlotChanged?.Invoke(item, count);
    }
}
