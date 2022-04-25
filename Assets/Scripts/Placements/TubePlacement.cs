using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
namespace FactoryGame.Placements
{
    [Obsolete("Bør ikke bruges, da den er forældet.", true)]
    public abstract class TubePlacement : MonoBehaviour
    {
        public enum TubeRotation
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// Den mængde items den kan holde hvert sekund.
        /// </summary>
        public float ItemCapacity;

        public UnityEvent<string> TubeIndexingChanged;

        public Dictionary<Orientation, (TubePlacement tube, Orientation tubeOut)> connectedTubeByOutDirection = new Dictionary<Orientation, (TubePlacement tube, Orientation tubeIn)>();

        public abstract Orientation InRotation { get; }
        public abstract Orientation OutRotation { get; }

        public abstract Orientation? GetOutTubeRotation(Orientation inRotation);

        public abstract void UpdateCorrespondingTubes();

        protected Dictionary<Orientation, Orientation> CorrespondingTubes { get; } = new Dictionary<Orientation, Orientation>();

        public Orientation? GetCorrespondingRotation(Orientation rotation)
        {
            if (CorrespondingTubes.TryGetValue(rotation, out Orientation oppositeRotation))
            {
                return oppositeRotation;
            }

            return null;
        }

        public void TubePlaced()
        {
            UpdateCorrespondingTubes();
            
            IndexTubes();
        }

        public void TubeRotated()
        {
            UpdateCorrespondingTubes();

            Debug.Log("Tube rotated");
            IndexTubes();
        }

        public void OutputTargetTubeUpdated()
        {
            Debug.Log("Output target tube updated");
            IndexTubes();
        }

        public void DesignateIndex(int index)
        {
            TubeIndexingChanged.Invoke(index.ToString());
        }

        /// <summary>
        /// Walks back along tubes to first out-tube,
        /// then performs indexing on all tubes after that one.
        /// </summary>
        void IndexTubes()
        {
            Debug.Log("Indexing connected tubes");

            // clear indexed tubes.
            connectedTubeByOutDirection.Clear();

            // Get (possibly) multiple tube in-directions.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                // Find kun de positioner med input.
                if ((InRotation & orientation) == 0)
                {
                    continue;
                }

                //Orientation oppositePosition = Placement.Rotate(orientation, 2);

                (TubePlacement tube, Orientation tubeIn) = FindInTube(orientation);

                Orientation? correspondingTube = tube.GetCorrespondingRotation(tubeIn);
                tube.IndexOutTube(correspondingTube!.Value, 0);
            }

            /*
            List<Orientation> intubeOrientations = new List<Orientation>();

            if (((byte)InRotation & (byte)Orientation.North) != 0)
            {
                intubeOrientations.Add(Orientation.North);
            }
            
            if (((byte)InRotation & (byte)Orientation.East) != 0)
            {
                intubeOrientations.Add(Orientation.East);
            }
            
            if (((byte)InRotation & (byte)Orientation.South) != 0)
            {
                intubeOrientations.Add(Orientation.South);

            }
            
            if (((byte)InRotation & (byte)Orientation.West) != 0)
            {
                intubeOrientations.Add(Orientation.West);
            }

            foreach (var item in intubeOrientations)
            {
                IndexOutTube(item, 0);
            }*/
        }

        /*public TubePlacement? IndexEarliestTube(Orientation localInPosition)
        {
            TubePlacement? tube;
            Vector2Int currentLocation = GridCalculation.GetTilePosition(transform.position);

            Orientation? correspondingPosition = GetCorrespondingRotation(localInPosition);

            if (correspondingPosition is null)
            {
                tube = null;
            }

            // is final tube
            if (tube is null)
            {
                // TODO: index instead of return.
                return this;
            }

            return tube.IndexEarliestTube();
        }*/

        public (TubePlacement tube, Orientation tubeIn) FindInTube(Orientation localInPosition)
        {
            Vector2Int currentLocation = GridCalculation.GetTilePosition(transform.position);
            Debug.Log("FindInTube: " + currentLocation);

            (TubePlacement? tube, Orientation tubeIn) = GetTubeConnectedIn(currentLocation, localInPosition);

            // ikke flere rør, returnér denne.
            if (tube is null)
            {
                return (this, tubeIn);
            }
            
            // fortsæt med at lede efter den længst væk
            return tube.FindInTube(tubeIn);
        }

        (TubePlacement? tube, Orientation distantConnectedPosition) GetTubeConnectedIn(Vector2Int currentLocation, Orientation localConnectedPosition)
        {
            // først led efter et rør i nærheden.
            Vector2Int lookupLocation = GridCalculation.GetNearbyTilePosition(currentLocation, localConnectedPosition);

            TubePlacement? tube = PlacementController.GetPlacement(lookupLocation)?.GetComponent<TubePlacement>();

            if (tube is null)
            {
                return (null, 0);
            }

            // Den position der skal matches er modsat den lokale position.
            Orientation remoteConnectedPosition = Placement.Rotate(localConnectedPosition, 2);


            //Debug.Log($"Placement lookup: {lookupLocation}, found: {tube?.gameObject?.name}");

            // Har det andet rør en udgang der hvor den kigger.
            if ((remoteConnectedPosition & tube.OutRotation) == 0)
            {
                return (null, 0);
            }

            Orientation? distantConnectedPosition = tube.GetCorrespondingRotation(remoteConnectedPosition);

            // Find den fjerne *in* rotation.
            if (distantConnectedPosition is null)
            {
                return (null, 0);
            }

            return (tube, distantConnectedPosition.Value);
        }

        public InValvePlacement? IndexOutTube(Orientation localOutPosition, int index)
        {
            Vector2Int currentLocation = GridCalculation.GetTilePosition(transform.position);

            Debug.Log(index);
            TubeIndexingChanged.Invoke(index.ToString());

            if (connectedTubeByOutDirection.TryGetValue(localOutPosition, out var connectedTube))
            {
                //if (connectedTube.tube is InValvePlacement valve)
                {
                    //return valve;
                }

                return connectedTube.tube.IndexOutTube(connectedTube.tubeOut, ++index);
            }

            (TubePlacement? tube, Orientation tubeIn) = GetTubeConnectedOut(currentLocation, localOutPosition);

            if (tube is null)
            {
                // Tube not found.
                return null;
            }

            Orientation? tubeOut = tube.GetCorrespondingRotation(tubeIn);

            if (tubeOut is null)
            {
                return null;
            }

            connectedTubeByOutDirection.Add(localOutPosition, (tube, tubeOut.Value));

            //if (tube is InValvePlacement inValve)
            {
                //return inValve;
            }

            return tube.IndexOutTube(tubeIn, ++index);
        }

        (TubePlacement? tube, Orientation tubeIn) GetTubeConnectedOut(Vector2Int currentLocation, Orientation localOutDirection)
        {
            Orientation matchInDirection = Placement.Rotate(localOutDirection, 2);

            Debug.Log($"Local out: {localOutDirection}, target in: {matchInDirection}");

            // skaffer rør som er i nærheden og peger i samme retning.
            Vector2Int lookupLocation = GridCalculation.GetNearbyTilePosition(currentLocation, localOutDirection);

            TubePlacement? tube = PlacementController.GetPlacement(lookupLocation)?.GetComponent<TubePlacement>();

            if (tube is null)
            {
                return (null, 0);
            }

            Debug.Log($"Placement lookup: {lookupLocation}, found: {tube.gameObject?.name}");

            Orientation? locatedTubeOutDirection = tube.GetOutTubeRotation(matchInDirection);

            // No corresponding out.
            if (locatedTubeOutDirection is null)
            {
                return (null, 0);
            }

            return (tube, matchInDirection);
        }
    }
}
