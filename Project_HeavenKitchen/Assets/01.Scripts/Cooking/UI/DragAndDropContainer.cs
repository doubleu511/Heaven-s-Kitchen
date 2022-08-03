using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropContainer : MonoBehaviour
{
    private Image image;
    public IngredientSO savedIngredient;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetIngredient(IngredientSO ingredient)
    {
        savedIngredient = ingredient;
        if (ingredient != null)
        {
            image.sprite = ingredient.ingredientMiniSpr;
        }
    }
}
