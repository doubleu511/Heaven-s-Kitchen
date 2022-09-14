using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoDishProcessUI : MonoBehaviour
{
    [SerializeField] Transform ingredientGridTrm;

    public void Refresh(RecipeSO recipe)
    {
        if (recipe.foodIngredient.isFood)
        {
            gameObject.SetActive(true);

            for (int i = 0; i < ingredientGridTrm.childCount; i++)
            {
                ingredientGridTrm.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < recipe.dishCraftRecipe.Length; i++)
            {
                MemoIngredientsPanelUI ingredientsPanel = Global.Pool.GetItem<MemoIngredientsPanelUI>();
                ingredientsPanel.Init(recipe.dishCraftRecipe[i]);
                ingredientsPanel.transform.SetParent(ingredientGridTrm);
                ingredientsPanel.transform.SetAsLastSibling();
            }

            UtilClass.ForceRefreshSize(transform);
            transform.SetAsLastSibling();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
