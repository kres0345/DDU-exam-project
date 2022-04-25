using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class MergerTubePlacement : TubePlacementV2
    {
        public override Orientation InPosition => Placement.Orientation |
                                                  Placement.Rotate(Placement.Orientation, 1) |
                                                  Placement.Rotate(Placement.Orientation, 3);

        public override Orientation OutPosition => Placement.Rotate(Placement.Orientation, 2);

        public override void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();

            CorrespondingTubes[OutPosition] = InPosition;

            foreach (Orientation position in Enum.GetValues(typeof(Orientation)))
            {
                if ((position & InPosition) == 0)
                {
                    continue;
                }

                CorrespondingTubes[position] = OutPosition;
            }
        }
    }
}
