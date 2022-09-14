using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DishUIHandler : MonoBehaviour
{
    private bool isFade = false;
    private CanvasGroup canvasGroup;
    [SerializeField] DragableUI[] dishTabs;
    [SerializeField] Button submitBtn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

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

    public void UIFade(bool value)
    {
        Global.UI.UIFade(canvasGroup, value);
        if (isFade && !value)
        {
            ReturnToInventory();
        }

        isFade = value;
    }

    private void ReturnToInventory()
    {
        for (int i = 0; i < dishTabs.Length; i++)
        {
            CookingManager.Player.Inventory.InventoryAdd(dishTabs[i].myIngredient);
        }

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
                if(AddDishFood(recipes[i].foodIngredient))
                {
                    UIFade(false);
                    return;
                }
            }
        }
    }

    private bool AddDishFood(IngredientSO ingredient)
    {
        if(CookingManager.Player.Inventory.InventoryAdd(ingredient, out int addIndex))
        {
            TabInfo info = new TabInfo();
            info.isDish = true;
            CookingManager.Player.Inventory.inventoryTabs[addIndex].SetInfo(info);

            for (int i = 0; i < dishTabs.Length; i++)
            {
                dishTabs[i].SetIngredient(null);
            }

            return true;
        }

        return false;
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
