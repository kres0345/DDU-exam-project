using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements.Exceptions
{
    public class InvalidItemTypeException : Exception
    {
        public Item ExpectedItem;
        public Item Item;

        public InvalidItemTypeException(Item item, Item expectedItem)
        {
            Item = item;
            ExpectedItem = expectedItem;
        }
    }
}
