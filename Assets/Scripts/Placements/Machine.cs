using System.Linq;
using System.Collections;
using UnityEngine;
using FactoryGame.Misc;
using FactoryGame.UI;
using UnityEngine.Events;

namespace FactoryGame.Placements
{
    public class Machine : MonoBehaviour, IPlacementSerializer
    {
        public Recipe[] Recipes;
        public Recipe SelectedRecipe;

        bool producing = false;
        public ContainerInventory InputInventory;
        public ContainerInventory OutputInventory;

        CraftingProgressBox craftingProgress;

        // Start is called before the first frame update
        void Awake()
        {
            for (int i = 0; i < InputInventory.InventorySlots.Length; i++)
            {
                InputInventory.InventorySlots[i].Capacity = 0;
            }

            craftingProgress = GetComponent<CraftingProgressBox>();
            craftingProgress.ResetProgressBox();
        }

        public void PlacementRotated()
        {
            RotateDisplaySlots();
        }

        public void PlacementPlaced()
        {
            for (int i = 1; i < (byte)GetComponent<Placement>().Orientation; i *= 2)
            {
                RotateDisplaySlots();
            }
        }

        public void RotateDisplaySlots()
        {
            foreach (ItemSlot itemSlot in InputInventory.InventorySlots.Concat(OutputInventory.InventorySlots).ToArray())
            {
                RectTransform rect = itemSlot.GetComponent<RectTransform>();
                rect.localPosition = new Vector2(rect.localPosition.y, rect.localPosition.x * (-1));
            }
        }

        public void SetRecipe(Recipe recipe)
        {
            SelectedRecipe = recipe;
            for (int i = 0; i < InputInventory.InventorySlots.Length; i++)
            {
                ItemSlot itemSlot = InputInventory.InventorySlots[i];
                itemSlot.Filter = SelectedRecipe.InputItems[i];
                itemSlot.Capacity = ItemSlot.MaxStackSize;

                DisplayItemSlot displaySlot = itemSlot.GetComponent<DisplayItemSlot>();
                displaySlot.recipeAmount = SelectedRecipe.InputItemCount[i];
                displaySlot.UpdateItemSlotAppearence();
            }

            OutputInventory.InventorySlots[0].Filter = SelectedRecipe.OutputItem;
            OutputInventory.InventorySlots[0].GetComponent<DisplayItemSlot>().UpdateItemSlotAppearence();
        }

        // Update is called once per frame
        void Update()
        {
            if (!producing && !(SelectedRecipe is null))
            {
                if (CheckProductionAvailability())
                {
                    producing = true;
                    for (int i = 0; i < SelectedRecipe.InputItems.Length; i++)
                    {
                        InputInventory.ConsumeItems(SelectedRecipe.InputItems[i], SelectedRecipe.InputItemCount[i]);
                    }
                    StartCoroutine(ProductionTime());
                }
            }
        }

        private IEnumerator ProductionTime()
        {
            SetRecipe(SelectedRecipe);
            craftingProgress.StartRecipeProgress(SelectedRecipe.ProcessingTime);
            yield return new WaitForSeconds(SelectedRecipe.ProcessingTime);
            craftingProgress.ResetProgressBox();

            OutputInventory.AddItems(SelectedRecipe.OutputItem, SelectedRecipe.OutputItemCount);
            producing = false;
        }

        private bool CheckProductionAvailability()
        {
            bool ableToProduce = true;

            for (int i = 0; i < SelectedRecipe.InputItems.Length; i++)
            {
                if (InputInventory.GetItemAmount(SelectedRecipe.InputItems[i]) < SelectedRecipe.InputItemCount[i])
                {
                    ableToProduce = false;
                }
            }

            if (OutputInventory.GetAvailableSpace(SelectedRecipe.OutputItem) < 1)
            {
                ableToProduce = false;
            }

            return ableToProduce;
        }

        public string GetSerializedData()
        {
            if (SelectedRecipe is null)
            {
                return string.Empty;
            }

            return SelectedRecipe.name;
        }

        public void SetSerializedData(string serializedData)
        {
            if (string.IsNullOrEmpty(serializedData))
            {
                return;
            }

            Recipe recipe = ItemController.Instance.GetRecipeByName(serializedData);
            SetRecipe(recipe);
        }
    }
}



