using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IngredientInventory : MonoBehaviour
{
    [SerializeField] Image ingredientImg;
    [SerializeField] Image fadeImg;

    public void InitIngredient(IngredientSO ingredient)
    {
        ingredientImg.sprite = ingredient.ingredientMiniSpr;
    }

    public void SetFade(bool value)
    {
        fadeImg.gameObject.SetActive(!value);
    }
}
