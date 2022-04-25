using UnityEngine;
using System.Linq;

namespace FactoryGame.Placements
{
    public class ItemController : MonoBehaviour
    {
        public static ItemController Instance;

        private void Awake()
        {
            Debug.Assert(Instance == null, "Der er allerede en ItemController i scenen");

            Instance = this;
        }

        public Item[] Items;
        public Recipe[] Recipes;

        public Recipe? GetRecipeByName(string name) => Recipes.FirstOrDefault(r => r.name == name);

        public Item? GetItemByName(string name) => Items.FirstOrDefault(i => i.Name == name);
    }
}
