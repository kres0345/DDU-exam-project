using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class DebugItemSlot : MonoBehaviour
    {
        public ItemSlot ItemSlot;
        public Item StartItem;
        public int StartCount;

        // Start is called before the first frame update
        void Awake()
        {
            ItemSlot.SetContent(StartItem, StartCount);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
