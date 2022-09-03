using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInventoryUI : MonoBehaviour
{
    [SerializeField] NeedyDragableUI ingredientImg;
    [SerializeField] Image fadeImg;

    private int minigameIndex;
    private int ingredientIndex;

    private void Start()
    {
        ingredientImg.onPrepareItem += (x) =>
        {
            PutUtensilsInventory(x);
            SetFade(x);

            MemoHandler memoHandler = FindObjectOfType<MemoHandler>();
            memoHandler.RefreshMinigameRecipes();
        };
    }

    private void PutUtensilsInventory(bool value)
    {
        if(value)
        {
            CookingManager.Global.CurrentUtensils.utensilsInventories[minigameIndex].ingredients[ingredientIndex] = ingredientImg.myIngredient;
        }
        else
        {
            CookingManager.Global.CurrentUtensils.utensilsInventories[minigameIndex].ingredients[ingredientIndex] = null;
        }

        CookingManager.Global.CurrentUtensils.RefreshInventoryGUI();
    }

    public void InitIngredient(IngredientSO ingredient, int minigameIdx, int ingredientIdx)
    {
        ingredientImg.SetIngredient(ingredient);

        minigameIndex = minigameIdx;
        ingredientIndex = ingredientIdx;
    }

    public void CleanInventory()
    {
        ingredientImg.onPrepareItem(false);
    }

    public void SetFade(bool value)
    {
        ingredientImg.beginDragLock = !value;
        fadeImg.gameObject.SetActive(!value);
    }
}
