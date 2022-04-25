using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
namespace FactoryGame.Placements
{
    public abstract class TubePlacementV2 : MonoBehaviour
    {
        public UnityEvent<string> TubeIndexingChanged;

        public abstract Orientation InPosition { get; }

        public abstract Orientation OutPosition { get; }

        protected readonly Dictionary<Orientation, (TubePlacementV2 tube, Orientation tubeOut)> ConnectedTubeByDirection = new Dictionary<Orientation, (TubePlacementV2 tube, Orientation tubeOut)>();

        protected readonly Dictionary<Orientation, Orientation> CorrespondingTubes = new Dictionary<Orientation, Orientation>();

        public readonly Dictionary<Orientation, (TubePlacementV2 tube, Orientation tubeOut)> ConnectedTubeByOutDirection = new Dictionary<Orientation, (TubePlacementV2 tube, Orientation tubeIn)>();

        public virtual Vector2Int TubeGridPosition => Placement.GridPosition;

        public Placement Placement;
        public AudioSource AudioSource;

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Orientation particularDirection in Enum.GetValues(typeof(Orientation)))
            {
                if ((particularDirection & OutPosition) == 0)
                {
                    continue;
                }

                Vector2Int position = GridCalculation.GetNearbyTilePosition(TubeGridPosition, particularDirection);
                Gizmos.DrawWireCube(new Vector3(position.x + 0.5f, position.y + 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f));
            }

            Gizmos.color = Color.green;
            foreach (Orientation particularDirection in Enum.GetValues(typeof(Orientation)))
            {
                if ((particularDirection & InPosition) == 0)
                {
                    continue;
                }

                Vector2Int position = GridCalculation.GetNearbyTilePosition(TubeGridPosition, particularDirection);
                Gizmos.DrawWireCube(new Vector3(position.x + 0.49f, position.y + 0.49f, 0), new Vector3(0.49f,0.49f, 0.5f));
            }
        }

        public void PlayHumSound()
        {
            AudioSource?.Play();
        }

        public void StopHumSound()
        {
            AudioSource?.Stop();
        }

        public Orientation? GetCorrespondingRotation(Orientation rotation)
        {
            if (CorrespondingTubes.TryGetValue(rotation, out Orientation oppositeRotation))
            {
                return oppositeRotation;
            }

            return null;
        }

        void IndexTubes()
        {
            // clear indexed tubes.
            ConnectedTubeByDirection.Clear();

            // Get (possibly) multiple tube in-directions.
            foreach (Orientation particularInPosition in Enum.GetValues(typeof(Orientation)))
            {
                if ((particularInPosition & InPosition) == 0)
                {
                    continue;
                }

                TubePlacementV2 primaryTube = FindPrimaryOutTube(this, particularInPosition);

                foreach (Orientation particularOutPosition in Enum.GetValues(typeof(Orientation)))
                {
                    if ((particularOutPosition & primaryTube.OutPosition) == 0)
                    {
                        continue;
                    }

                    primaryTube.IndexOutTube(particularOutPosition, 0);
                }
            }
        }

        public static TubePlacementV2 FindPrimaryOutTube(TubePlacementV2 tube, Orientation particularInPosition)
        {
            Orientation nextConnectedPositon = particularInPosition;
            TubePlacementV2? currentTube = tube;

            // Er ikke sikker på denne forudsætning.
            while (!(currentTube is null))
            {
                // Er røret det sidste mulige rør?
                if (currentTube is OutValvePlacement)
                {
                    // Hvis ja, returnér
                    return currentTube;
                }

                // Find rørets position.
                Vector2Int tubePosition = currentTube.TubeGridPosition;
                
                // Gem det rør den kender nu
                TubePlacementV2? previousTube = currentTube;
                
                // Find det næste rør i rækken.
                currentTube = GetNearbyConnectedTube(tubePosition, nextConnectedPositon);

                // Blev røret fundet?
                if (currentTube is null)
                {
                    // Hvis ikke, så returnér det gemte rør.
                    return previousTube;
                }

                // Et rør blev fundet, men har den en valid passthru som liner op med dette rør?
                Orientation? newOtherConnection = currentTube.GetCorrespondingRotation(Placement.Rotate(nextConnectedPositon, 2));
                if (newOtherConnection is null)
                {
                    // Hvis nej, returnér det gemte rør.
                    return previousTube;
                }

                // Det matcher.
                nextConnectedPositon = newOtherConnection.Value;
            }

            return tube;
        }

        public (InValvePlacement? inValve, int index) IndexOutTube(Orientation localOutPosition, int index)
        {
            Vector2Int currentPosition = TubeGridPosition;

            TubeIndexingChanged.Invoke(index.ToString());

            if (ConnectedTubeByOutDirection.TryGetValue(localOutPosition, out var connectedTube))
            {
                if (connectedTube.tube == null)
                {
                    ConnectedTubeByOutDirection.Remove(localOutPosition);
                }
                else
                {
                    if (connectedTube.tube is InValvePlacement valve)
                    {
                        return (valve, index);
                    }

                    return connectedTube.tube.IndexOutTube(connectedTube.tubeOut, ++index);
                }
            }

            (TubePlacementV2? tube, Orientation tubeIn) = GetTubeConnectedOut(currentPosition, localOutPosition);

            if (tube is null)
            {
                // Tube not found.
                return (null, index);
            }

            Orientation? tubeOut = tube.GetCorrespondingRotation(tubeIn);

            if (tubeOut is null)
            {
                return (null, index);
            }

            ConnectedTubeByOutDirection.Add(localOutPosition, (tube, tubeOut.Value));

            if (tube is InValvePlacement inValve)
            {
                return (inValve, index);
            }

            return tube.IndexOutTube(tubeOut.Value, ++index);
        }

        public (TubePlacementV2? tube, Orientation tubeIn) GetTubeConnectedOut(Vector2Int currentLocation, Orientation localOutDirection)
        {
            Orientation matchInDirection = Placement.Rotate(localOutDirection, 2);

            // skaffer rør som er i nærheden og peger i samme retning.
            Vector2Int lookupLocation = GridCalculation.GetNearbyTilePosition(currentLocation, localOutDirection);

            Placement? gottonPlacement = PlacementController.GetPlacement(lookupLocation);

            if (gottonPlacement is null)
            {
                return (null, 0);
            }

            TubePlacementV2[]? tubes = gottonPlacement.GetComponentsInChildren<TubePlacementV2>();

            if (tubes is null || tubes.Length == 0)
            {
                return (null, 0);
            }

            foreach (var singleTube in tubes)
            {
                Orientation? locatedTubeOutPosition = singleTube.GetCorrespondingRotation(matchInDirection);

                // No corresponding out.
                if (locatedTubeOutPosition is null)
                {
                    continue;
                }

                return (singleTube, matchInDirection);
            }

            return (null, 0);
        }

        public static TubePlacementV2? GetNearbyConnectedTube(Vector2Int tubePosition, Orientation connectedPosition)
        {
            // Find den position der bør være et rør på.
            Vector2Int newTubeLocation = GridCalculation.GetNearbyTilePosition(tubePosition, connectedPosition);
            
            // Find selve røret (eller ingenting).
            // Skal bruge "GetComponents" da der kan være flere på én gang, i tilfælde af in og out valves.
            TubePlacementV2[]? tubes = PlacementController.GetPlacement(newTubeLocation)?.GetComponentsInChildren<TubePlacementV2>();

            // Er der tale om et rør?
            if (tubes is null || tubes.Length == 0)
            {
                // Nej, returnér ingenting.
                return null;
            }

            Orientation orientation = Placement.Rotate(connectedPosition, 2);

            foreach (var singleTube in tubes)
            {
                // Liner røret op med dette rør?
                if (singleTube.GetCorrespondingRotation(orientation) is null)
                {
                    continue;
                }

                // Nej, returnér ingenting, da dette rør ikke tæller.
                return singleTube;
            }

            return null;
        }

        public void PlacementPlaced()
        {
            UpdateCorrespondingTubes();
            IndexTubes();
        }

        public void PlacementRotated()
        {
            UpdateCorrespondingTubes();
            IndexTubes();
        }

        public void DesignateIndex(int index)
        {
            TubeIndexingChanged.Invoke(index.ToString());
        }

        public virtual void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();

            var a = (InPosition, OutPosition);
            CorrespondingTubes[a.Item1] = a.Item2;
            CorrespondingTubes[a.Item2] = a.Item1;
        }
    }
}
