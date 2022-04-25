namespace FactoryGame.Placements
{
    public class StraightTubePlacement : TubePlacementV2
    {
        public override Orientation InPosition => Placement.Orientation;

        // Shift twice to get cardinally opposite direction.
        public override Orientation OutPosition => Placement.Rotate(Placement.Orientation, 2);
    }
}
