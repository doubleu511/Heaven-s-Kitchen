using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterIngredientInventoryUI2 : MonoBehaviour
{
    [SerializeField] ShowLoreDragableUI ingredientImg;
    [SerializeField] Image fadeImg;
    [SerializeField] Image dishImg;

    public void Init(IngredientSO ingredient)
    {
        dishImg.gameObject.SetActive(ingredient.isFood);

        TabInfo info = new TabInfo();
        info.isDish = ingredient.isFood;
        ingredientImg.SetTabInfo(info);

        ingredientImg.SetIngredient(ingredient);
        ingredientImg.isRequired = true;
        SetFade(false);
    }


    public void SetFade(bool value)
    {
        fadeImg.gameObject.SetActive(!value);
    }
}
