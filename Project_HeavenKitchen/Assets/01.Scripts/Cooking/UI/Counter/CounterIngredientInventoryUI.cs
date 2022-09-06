using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterIngredientInventoryUI : MonoBehaviour
{
    [SerializeField] NeedyDragableUI ingredientImg;
    [SerializeField] Image fadeImg;
    private bool isFinished = false;
    private List<CounterIngredientInventoryUI> counterIngredientInventories;

    private void Start()
    {
        ingredientImg.onPrepareItem += (x) =>
        {
            ingredientImg.beginDragLock = true;
            SetFade(x);
            isFinished = x;
            CookingManager.Counter.AddDish(ingredientImg.myIngredient);
            //�̰��� �߰��Ǿ����� ������ ���� �� �˻�

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
                // �ֹ� Ŭ����
                CookingManager.Counter.OrderClear();
            }
        };
    }

    public void Init(IngredientSO ingredient, List<CounterIngredientInventoryUI> counterIngredientList)
    {
        ingredientImg.SetIngredient(ingredient);
        counterIngredientInventories = counterIngredientList;
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
