using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactoryGame.Placements;

namespace FactoryGame.UI
{
    public class CraftingQueue : MonoBehaviour
    {
        bool crafting = false;
        Queue<Queueue> queueues = new Queue<Queueue>();

        public DisplayItemSlot[] QueueSlots;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < QueueSlots.Length; i++)
            {
                QueueSlots[i].transform.GetChild(2).transform.localScale = new Vector2(1, 0);
            }
        }

        public void AddRecipe(Recipe recipe, int craftingAmount)
        {
            QueueSlots[queueues.Count].ItemSlot.Item = recipe.OutputItem;
            QueueSlots[queueues.Count].ItemSlot.Count = craftingAmount;
            queueues.Enqueue(new Queueue(recipe, craftingAmount));
        }

        public bool QueueOccupied()
        {
            return queueues.Count > 6;
        }

        IEnumerator Crafting()
        {
            Recipe recipe = queueues.Peek().Recipe;
            Transform boxTransform = QueueSlots[0].transform.GetChild(2).transform;
            boxTransform.LeanScaleY(1, recipe.ProcessingTime);

            yield return new WaitForSeconds(recipe.ProcessingTime);

            boxTransform.localScale = new Vector2(1, 0);

            if (queueues.Peek().CraftAmount == 1)
            {
                queueues.Dequeue();

                for (int i = 0; i < QueueSlots.Length - 1; i++)
                {
                    QueueSlots[i].ItemSlot.Item = QueueSlots[i + 1].ItemSlot.Item;
                    QueueSlots[i].ItemSlot.Count = QueueSlots[i + 1].ItemSlot.Count;
                }
                QueueSlots[6].ItemSlot.Item = null;
            }
            else
            {
                queueues.Peek().CraftAmount--;
                QueueSlots[0].ItemSlot.Count = queueues.Peek().CraftAmount;
            }

            crafting = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!crafting && queueues.Count > 0)
            {
                crafting = true;
                StartCoroutine(Crafting());
            }
        }
    }
}
