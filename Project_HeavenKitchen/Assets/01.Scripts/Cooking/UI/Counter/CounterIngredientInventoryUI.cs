using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterIngredientInventoryUI : MonoBehaviour
{
    [SerializeField] NeedyDragableUI ingredientImg;
    [SerializeField] Image fadeImg;
    [SerializeField] Image dishImg;
    private bool isFinished = false;
    private List<CounterIngredientInventoryUI> counterIngredientInventories;

    private void Start()
    {
        ingredientImg.onPrepareItem += (x) =>
        {
            ingredientImg.beginDragLock = true;
            SetFade(x);
            isFinished = x;
            //이곳에 추가되었을때 레시피 갱신 및 검사
            CookingManager.Counter.AddDish(ingredientImg.myIngredient);
            CookingManager.Global.MinusAllIngredients(ingredientImg.myIngredient);

            bool isOrderEnd = true;

            for (int i = 0; i < counterIngredientInventories.Count; i++)
            {
                if (!counterIngredientInventories[i].isFinished)
                {
                    isOrderEnd = false;
                }
            }

            if (isOrderEnd)
            {
                // 주문 클리어
                CookingManager.Counter.OrderClear();
            }

            MemoHandler memoHandler = FindObjectOfType<MemoHandler>();
            memoHandler.RefreshMinigameRecipes();
        };
    }

    public void Init(IngredientSO ingredient, List<CounterIngredientInventoryUI> counterIngredientList)
    {
        isFinished = false;

        dishImg.gameObject.SetActive(ingredient.isFood);
        counterIngredientInventories = counterIngredientList;

        TabInfo info = new TabInfo();
        info.isDish = ingredient.isFood;
        ingredientImg.SetTabInfo(info);

        ingredientImg.SetIngredient(ingredient);
        ingredientImg.beginDragLock = true;
        SetFade(false);
    }

    public void CleanInventory()
    {
        ingredientImg.onPrepareItem(false);
    }

    public void SetFade(bool value)
    {
        fadeImg.gameObject.SetActive(!value);
    }
}
