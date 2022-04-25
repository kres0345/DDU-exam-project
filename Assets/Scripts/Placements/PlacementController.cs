using FactoryGame;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    public static class PlacementController
    {
        static readonly Dictionary<Vector2Int, Placement> placements = new Dictionary<Vector2Int, Placement>();

        public static void ResetStatics()
        {
            placements.Clear();
        }

        public static void PlaceBuilding(Placement placement, Vector2Int position)
        {
            placement.UpdateSprite(false);
            placement.GridPosition = position;

            RegisterPlacement(placement, position);

            Debug.Assert(position == placement.GridPosition, $"Konstruret {position} != grid {placement.GridPosition}");

            placement.Placed = true;
            placement.TriggerPlacementPlaced();
        }

        public static Dictionary<Vector2Int, Placement> GetPlacements() => placements;

        public static void DeregisterPlacement(Placement placement, Vector2Int position)
        {
            for (int x = 0; x < placement.BlockWidth; x++)
            {
                for (int y = 0; y < placement.BlockHeight; y++)
                {
                    Vector2Int exactPosition = new Vector2Int(x, y) + position;
                    placements.Remove(exactPosition);
                }
            }
        }

        public static Placement? GetPlacement(Vector2Int position)
        {
            if (placements.TryGetValue(position, out Placement value))
            {
                return value;
            }

            return null;
        }

        private static void RegisterPlacement(Placement placement, Vector2Int position)
        {
            for (int x = 0; x < placement.BlockWidth; x++)
            {
                for (int y = 0; y < placement.BlockHeight; y++)
                {
                    Vector2Int exactPosition = new Vector2Int(x, y) + position;

                    placements[exactPosition] = placement;
                }
            }
        }
    }
}
