using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FactoryGame.Placements;
using FactoryGame.Placements;

public class ItemRecipePanel : MonoBehaviour
{
    public Image ItemIcon;
    public TextMeshProUGUI ItemName;
    private Recipe recipe;
    public Machine MachineReference;
    public Recipe CurrentRecipe
    {
        get => recipe;
        set
        {
            recipe = value;
            ItemIcon.sprite = value.OutputItem.Icon;
            ItemIcon.color = new Color(1, 1, 1, 1);
            ItemName.text = $"{value.OutputItem.Name}\nProcess time: {value.ProcessingTime}     Amount: {value.OutputItemCount}";
        }
    }

    public void SelectRecipe()
    {
        MachineReference.SetRecipe(CurrentRecipe);
        MachineRecipeSelectView.Instance.HideSelectRecipeView();
    }
}
