using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtensilsCircleInventoryUI : MonoBehaviour
{
    [SerializeField] Image iconImg;

    public void Init(IngredientSO ingredient)
    {
        iconImg.sprite = ingredient.ingredientMiniSpr;
    }

    public void SetPlus()
    {
        iconImg.sprite = UtensilsUIHandler.PlusSpr;
    }
}
