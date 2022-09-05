using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterDishUI : MonoBehaviour
{
    [SerializeField] Image dishImg;
    [SerializeField] Image foodImg;

    public void Init(IngredientSO ingredient)
    {
        foodImg.sprite = ingredient.ingredientDefaulrSpr;
    }
}
