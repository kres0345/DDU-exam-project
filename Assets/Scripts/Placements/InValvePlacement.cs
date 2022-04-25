using FactoryGame.Placements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    public class InValvePlacement : ValveBasePlacement
    {
        public override Orientation InPosition => Placement.Rotate(ActualValveOrientation, 2);

        public override Orientation OutPosition => 0;

        public override void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();
            CorrespondingTubes[InPosition] = 0;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector2Int position = GridCalculation.GetNearbyTilePosition(TubeGridPosition, Placement.Rotate(ActualValveOrientation, 2));
            Gizmos.DrawWireCube(new Vector3(position.x + 0.5f, position.y + 0.5f, 0), new Vector3(1.1f, 1.1f, 0.5f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proposedItems"></param>
        /// <returns></returns>
        public ItemTransaction? CreateItemTransaction(IEnumerable<ItemSlot> proposedItems)
        {
            var receivingItemSlots =
                (from slot in ContainerInventory.ItemSlots
                where !slot.Full
                orderby (slot.Item ?? slot.Filter) is null
                select slot).ToList();

            foreach (var outSlot in proposedItems)
            {
                if (outSlot.Item is null)
                {
                    continue;
                }

                if (outSlot.AvailableCount == 0)
                {
                    continue;
                }

                foreach (var inSlot in receivingItemSlots)
                {
                    if (inSlot.SlotAvailable(outSlot.Item))
                    {
                        return new ItemTransaction(outSlot, inSlot);
                    }
                }
            }

            return null;
        }
    }
}
