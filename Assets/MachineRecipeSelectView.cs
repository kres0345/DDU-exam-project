using UnityEngine;
using FactoryGame.UI;
using FactoryGame.Placements;
using FactoryGame.Placements;

public class MachineRecipeSelectView : MonoBehaviour
{
    public static MachineRecipeSelectView Instance { get; private set; }

    public GameObject ItemPanel;
    public Transform Content;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void HideSelectRecipeView()
    {
        gameObject.SetActive(false);
    }

    public void ShowSelectRecipeView(Machine machine)
    {
        Debug.Log(Content.childCount);
        for (int i = Content.childCount - 1; i >= 0; i--)
        {
            Transform child = Content.transform.GetChild(i);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        Debug.Log(Content.childCount);

        Recipe[] recipes = machine.Recipes;

        for (int i = 0; i < recipes.Length; i++)
        {
            GameObject newItemPanel = Instantiate(ItemPanel);
            newItemPanel.transform.SetParent(Content);
            newItemPanel.transform.localScale = new Vector2(1, 1);

            ItemRecipePanel recipePanel = newItemPanel.GetComponent<ItemRecipePanel>();
            recipePanel.CurrentRecipe = recipes[i];
            recipePanel.MachineReference = machine;
        }

        gameObject.SetActive(true);
        Content.GetComponent<ScrollViewContentController>().UpdateContentSize();
    }
}
