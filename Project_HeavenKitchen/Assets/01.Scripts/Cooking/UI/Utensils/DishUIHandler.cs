using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DishUIHandler : MonoBehaviour
{
    [SerializeField] DragableUI[] dishTabs;
    [SerializeField] Button submitBtn;

    private void Start()
    {
        submitBtn.onClick.AddListener(() =>
        {
            CheckRecipeDish();
        });

        for (int i = 0; i < dishTabs.Length; i++)
        {
            dishTabs[i].SetIngredient(null);
        }
    }

    private void CheckRecipeDish()
    {
        IngredientSO[] tabIngredients = GetIngredientAndSort();

        RecipeSO[] recipes = CookingManager.GetRecipes();
        for (int i = 0; i < recipes.Length; i++)
        {
            IngredientSO[] recipeDishRecipe = GetIngredientAndSort(recipes[i].dishCraftRecipe);

            if (tabIngredients.SequenceEqual(recipeDishRecipe))
            {
                Debug.Log("True : " + recipes[i].foodIngredient);
                AddDishFood(recipes[i].foodIngredient);

            }
        }
    }

    private void AddDishFood(IngredientSO ingredient)
    {
        if(CookingManager.Player.Inventory.InventoryAdd(ingredient, out int addIndex))
        {
            CookingManager.Player.Inventory.inventoryTabs[addIndex].isDish = true;

            for (int i = 0; i < dishTabs.Length; i++)
            {
                dishTabs[i].SetIngredient(null);
            }
        }
    }

    private IngredientSO[] GetIngredientAndSort()
    {
        List<IngredientSO> ingredientList = new List<IngredientSO>();

        for (int i = 0; i < dishTabs.Length; i++)
        {
            if(dishTabs[i].myIngredient != null)
            {
                ingredientList.Add(dishTabs[i].myIngredient);
            }
        }

        ingredientList.Sort((x,y) => x.ingredientTranslationId.CompareTo(y.ingredientTranslationId));
        return ingredientList.ToArray();
    }

    private IngredientSO[] GetIngredientAndSort(IngredientSO[] ingredients)
    {
        Array.Sort(ingredients, (x, y) => x.ingredientTranslationId.CompareTo(y.ingredientTranslationId));

        return ingredients;
    }
}
