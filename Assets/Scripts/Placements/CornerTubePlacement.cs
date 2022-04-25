using UnityEngine;

namespace FactoryGame.Placements
{
    public class CornerTubePlacement : TubePlacementV2
    {
        public bool Counterclockwise;
        
        public override Orientation InPosition => Placement.Orientation;

        public override Orientation OutPosition => Placement.Rotate(Placement.Orientation, Counterclockwise ? 3 : 1);
    }
}
