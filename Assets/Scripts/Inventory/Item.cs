using UnityEngine;

namespace FactoryGame.Placements
{
    [CreateAssetMenu(fileName = "Item", menuName = "FactoryGame/Item")]
    public class Item : ScriptableObject
    {
        [Header("General")]
        public string Name;

        [Header("Inventory")]
        public Sprite Icon;
        public string Description;

        [Header("Placement")]
        public GameObject PlacementPrefab;
    }
}
