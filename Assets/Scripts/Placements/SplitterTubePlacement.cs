using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class SplitterTubePlacement : TubePlacementV2
    {
        public override Orientation InPosition => Placement.Orientation;

        public override Orientation OutPosition => Placement.Rotate(Placement.Orientation, 1) |
                                                   Placement.Rotate(Placement.Orientation, 2) |
                                                   Placement.Rotate(Placement.Orientation, 3);

        public override void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();

            CorrespondingTubes[InPosition] = OutPosition;

            foreach (Orientation position in Enum.GetValues(typeof(Orientation)))
            {
                if ((position & OutPosition) == 0)
                {
                    continue;
                }

                CorrespondingTubes[position] = InPosition;
            }
        }
    }
}
