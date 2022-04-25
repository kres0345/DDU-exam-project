using FactoryGame.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable enable
namespace FactoryGame.Placements
{
    [Flags]
    public enum Orientation : byte
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
    }

    [DisallowMultipleComponent]
    public sealed class Placement : MonoBehaviour
    {
        public int BlockWidth;
        public int BlockHeight;
        public Sprite North;
        public Sprite South;
        public Sprite East;
        public Sprite West;
        public bool Placed = false;

        public Orientation Orientation { get; set; } = Orientation.North;

        public Vector2Int GridPosition
        {
            get
            {
                return GridCalculation.GetTilePosition(transform.position - (new Vector3(BlockWidth, BlockHeight) / 2));
            }
            set
            {
                transform.position = value + (new Vector2(BlockWidth, BlockHeight) / 2);
            }
        }

        /// <summary>
        /// Called when a placement is being removed.
        /// </summary>
        //public UnityEvent PlacementRemoved;
        /// <summary>
        /// Called when a placement is placed (not during preview).
        /// </summary>
        //public UnityEvent PlacementPlaced;

        /// <summary>
        /// TODO: kald denne.
        /// </summary>
        //public UnityEvent PlacementRotated;

        public void TriggerPlacementRotated()
        {
            BroadcastMessage("PlacementRotated");
        }

        public void TriggerPlacementPlaced()
        {
            Placed = true;
            BroadcastMessage("PlacementPlaced");
        }

        public void TriggerPlacementRemoved()
        {
            BroadcastMessage("PlacementRemoved");
        }

        SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateSprite(bool preview)
        {
            Sprite sprite = null;
            switch (Orientation)
            {
                case Orientation.North:
                    sprite = North;
                    break;
                case Orientation.South:
                    sprite = South;
                    break;
                case Orientation.East:
                    sprite = East;
                    break;
                case Orientation.West:
                    sprite = West;
                    break;
                default:
                    Debug.Log("Fejl");
                    break;
            }

            if (sprite is null)
            {
                return;
            }

            spriteRenderer.color = new Color(1, 1, 1, preview ? 0.5f : 1f);
            spriteRenderer.sortingOrder = preview ? 2 : 0;
            spriteRenderer.sprite = sprite;
        }

        public static Orientation Rotate(Orientation orientation, int rotateAmount)
        {
            int mask = 15;

            return (Orientation)(
                (((byte)orientation << rotateAmount) | (byte)orientation >> (4 - rotateAmount)) & mask
                );
        }

        public string GetPrefabName() => gameObject.name.Replace("(Clone)", "");

        public string GetSerializedData()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append((byte)Orientation);
            sb.Append(';');

            foreach (var item in GetSerializableComponents())
            {
                string dataString = item.GetSerializedData();
                sb.Append(ToBase64(dataString));
                sb.Append(',');
            }

            return sb.ToString();
        }

        public void SetSerializedData(string serializedData)
        {
            string[] splitData = serializedData.Split(new char[] {';'}, 2);

            Orientation = (Orientation)byte.Parse(splitData[0]);

            string[] componentData = splitData[1].Split(',');

            int dataIndex = 0;
            foreach (var item in GetSerializableComponents())
            {
                string dataString = FromBase64(componentData[dataIndex]);
                item.SetSerializedData(dataString);

                dataIndex += 1;
            }
        }

        IEnumerable<IPlacementSerializer> GetSerializableComponents() => GetComponentsInChildren(typeof(IPlacementSerializer), true).Cast<IPlacementSerializer>();

        static string ToBase64(string source) => Convert.ToBase64String(Encoding.UTF8.GetBytes(source));

        static string FromBase64(string base64) => Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }
}
