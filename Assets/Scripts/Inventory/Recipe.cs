using UnityEngine;
using FactoryGame.Placements;

namespace FactoryGame.Placements
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "FactoryGame/Recipe")]
    public class Recipe : ScriptableObject
    {
        [Header("General")]
        public float ProcessingTime;

        [Header("Input")]
        public Item[] InputItems;
        public int[] InputItemCount;

        [Header("Output")]
        public Item OutputItem;
        public int OutputItemCount;
    }
}