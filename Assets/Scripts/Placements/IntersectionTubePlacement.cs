using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryGame.Placements
{
    public class IntersectionTubePlacement : TubePlacementV2
    {
        // Returns current direction and shifted direction.
        public override Orientation InPosition => Placement.Orientation | Placement.Rotate(Placement.Orientation, 1);

        // In rotation, but rotated once.
        public override Orientation OutPosition => Placement.Rotate(Placement.Orientation, 2) | Placement.Rotate(Placement.Orientation, 3);

        public override void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();
            
            var a = (Placement.Orientation, Placement.Rotate(Placement.Orientation, 2));
            var b = (Placement.Rotate(Placement.Orientation, 1), Placement.Rotate(Placement.Orientation, 3));

            CorrespondingTubes[a.Item1] = a.Item2;
            CorrespondingTubes[a.Item2] = a.Item1;

            CorrespondingTubes[b.Item1] = b.Item2;
            CorrespondingTubes[b.Item2] = b.Item1;
        }
    }
}
