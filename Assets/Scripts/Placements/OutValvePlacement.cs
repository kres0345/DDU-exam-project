using FactoryGame.Placements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    public class OutValvePlacement : ValveBasePlacement
    {
        public float TubeRechargeDelay = 1;
        public float PerTubeTime = 0.1f;

        ItemTransaction? currentTransaction;
        float delayFinishedTime;
        bool wasConnectedLastframe;

        public override Orientation InPosition => 0;

        public override Orientation OutPosition => ActualValveOrientation;

        void FixedUpdate()
        {
            // Ingen logik skal finde sted før den er placeret.
            if (!Placement.Placed)
            {
                return;
            }

            if (ContainerInventory is null)
            {
                // Not supposed to happen.
                return;
            }

            //(InValvePlacement? inValve, int index) = IndexOutTube(OutRotation, 0);
            // TODO: sandsynligvis ikke særligt performant at danne en liste hver update.
            List<TubePlacementV2> connectedTubes = GetConnectedTubes().ToList();

            InValvePlacement? inValve = connectedTubes[connectedTubes.Count - 1] as InValvePlacement;

            // no connection
            if (inValve is null)
            {
                //Debug.Log("Not connected: " + connectedTubes.Count);
                // connection has ended
                if (wasConnectedLastframe)
                {
                    // flawed, if a tube is broken apart, then the end will still play.
                    foreach (var item in connectedTubes)
                    {
                        item.StopHumSound();
                    }
                }

                wasConnectedLastframe = false;
                return; // ved godt hvad jeg sagde Jonathan, men her er alternativet slemt (cyklomatisk set)
            }

            // connection has been made
            if (!wasConnectedLastframe)
            {
                delayFinishedTime = Time.time + TubeRechargeDelay;

                // rørene er forbundet til en indgang, så de kan godt starte deres lyd.
                foreach (var item in connectedTubes)
                {
                    item.PlayHumSound();
                }
            }

            wasConnectedLastframe = true;

            // Opret transaktion, hvis tiden er gået.
            if (currentTransaction is null)
            {
                if (Time.time > delayFinishedTime)
                {
                    currentTransaction = inValve.CreateItemTransaction(ContainerInventory.ItemSlots.Where(s => !(s.Item is null)));

                    if (!(currentTransaction is null))
                    {
                        currentTransaction.StartTransaction(Time.time + (PerTubeTime * (connectedTubes.Count - 1)));
                    }
                }
            }
            else // Tjek om nuværende transaktioner er færdige
            {
                //Debug.Log($"Item transaction created and executing in {currentTransaction.ExecutionTimestamp - Time.time}");

                // finish transactions that are completed.
                if (Time.time > currentTransaction.ExecutionTimestamp)
                {
                    currentTransaction.FinishTransaction();
                    currentTransaction = null;
                    delayFinishedTime = Time.time + TubeRechargeDelay;
                }
            }
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector2Int position = GridCalculation.GetNearbyTilePosition(TubeGridPosition, ActualValveOrientation);
            Gizmos.DrawWireCube(new Vector3(position.x + 0.5f, position.y + 0.5f, 0), new Vector3(1, 1, 0.5f));
        }

        public override void UpdateCorrespondingTubes()
        {
            CorrespondingTubes.Clear();
            CorrespondingTubes[0] = OutPosition;
        }

        /// <summary>
        /// Returns enumerable of all tubes connected including valves.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TubePlacementV2> GetConnectedTubes()
        {
            //IndexOutTube(OutRotation, 0);
            Orientation currentOutPosition = OutPosition;
            TubePlacementV2 currentTube = this;

            int index = 0;

            while (true)
            {
                yield return currentTube;

                currentTube.TubeIndexingChanged.Invoke(index++.ToString());

                if (currentTube is InValvePlacement)
                {
                    yield break;
                }

                // check cache
                if (currentTube.ConnectedTubeByOutDirection.TryGetValue(currentOutPosition, out var connectedTube))
                {
                    if (connectedTube.tube == null)
                    {
                        currentTube.ConnectedTubeByOutDirection.Remove(currentOutPosition);
                    }
                    else
                    {
                        currentTube = connectedTube.tube;
                        currentOutPosition = connectedTube.tubeOut;
                        continue;
                    }
                }

                Vector2Int currentPosition = currentTube.TubeGridPosition;
                //Debug.Log($"Currenttube: {currentPosition}, {currentOutPosition}");

                (TubePlacementV2? tube, Orientation tubeIn) = currentTube.GetTubeConnectedOut(currentPosition, currentOutPosition);

                if (tube is null)
                {
                    // Tube not found.
                    yield break;
                }

                Orientation? tubeOut = tube.GetCorrespondingRotation(tubeIn);

                if (tubeOut is null)
                {
                    yield break;
                }

                currentTube.ConnectedTubeByOutDirection.Add(currentOutPosition, (tube, tubeOut.Value));

                currentOutPosition = tubeOut.Value;
                currentTube = tube;
            }
        }
    }
}
