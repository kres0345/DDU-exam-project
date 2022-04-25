using UnityEngine;

namespace FactoryGame.UI
{
    public class CraftingProgressBox : MonoBehaviour
    {
        public Transform boxTransform;

        public void StartRecipeProgress(float processingTime)
        {
            boxTransform.LeanScaleY(1, processingTime);
        }

        public void ResetProgressBox()
        {
            boxTransform.localScale = new Vector2(1, 0);
        }
    }
}
