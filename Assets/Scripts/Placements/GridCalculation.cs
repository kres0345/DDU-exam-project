using System.Collections;
using UnityEngine;

namespace FactoryGame.Placements
{
    public static class GridCalculation
    {
        public static Vector2Int GetTilePosition(Vector2 position)
        {
            int x = Mathf.FloorToInt(position.x);
            int y = Mathf.FloorToInt(position.y);

            return new Vector2Int(x, y);
        }

        public static Vector2Int GetNearbyTilePosition(Vector2Int position, Orientation direction)
        {
            switch (direction)
            {
                case Orientation.North:
                    return position + new Vector2Int(0, 1);
                case Orientation.East:
                    return position + new Vector2Int(1, 0);
                case Orientation.South:
                    return position + new Vector2Int(0, -1);
                case Orientation.West:
                    return position + new Vector2Int(-1, 0);
                default:
                    break;
            }

            Debug.LogError("Unexpected enum value");
            return Vector2Int.zero;
        }
    }
}
