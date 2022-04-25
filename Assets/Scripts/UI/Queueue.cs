using FactoryGame.Placements;

namespace FactoryGame.UI
{
    public class Queueue
    {
        public Recipe Recipe;
        public int CraftAmount;

        public Queueue(Recipe recipe, int craftAmount)
        {
            Recipe = recipe;
            CraftAmount = craftAmount;
        }
    }
}
