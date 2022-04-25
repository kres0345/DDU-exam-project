using FactoryGame.Placements;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    public abstract class ValveBasePlacement : TubePlacementV2
    {
        public ContainerInventory? ContainerInventory;
        public Vector2Int DefaultRelativePosition;
        /// <summary>
        /// The in- and out-valve orientation. Flipped for invalve. The orientation for other tubes to match.
        /// </summary>
        public Orientation DefaultValveOrientation;
        public Orientation ActualValveOrientation
        { 
            get
            {
                // Flipped valve orientation for in-valve.
                Orientation valveOrientation = DefaultValveOrientation;
                for (int i = 1; i < (byte)Placement.Orientation; i *= 2)
                {
                    valveOrientation = Placement.Rotate(valveOrientation, 1);
                }
                return valveOrientation;
            }
        }

        public override Vector2Int TubeGridPosition => base.TubeGridPosition + ActualRelativePosition;

        public Vector2Int ActualRelativePosition
        {
            get
            {
                Vector2 centerVector = new Vector2(Placement.BlockWidth - 1, Placement.BlockHeight - 1) / 2;
                Vector2 relativeToCenter = DefaultRelativePosition - centerVector;

                for (int i = 1; i < (byte)Placement.Orientation; i *= 2)
                {
                    relativeToCenter = new Vector2(relativeToCenter.y, relativeToCenter.x * (-1));
                }

                Vector2 relativePosition = relativeToCenter + centerVector;

                return new Vector2Int((int)relativePosition.x, (int)relativePosition.y);
            }
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            Debug.Assert(!(ContainerInventory is null), $"{transform.name}: {nameof(ContainerInventory)} is not assigned a value.");
        }
    }
}
