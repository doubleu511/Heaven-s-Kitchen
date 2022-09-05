using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropContainer : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Image image;
    public IngredientSO savedIngredient;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetIngredient(IngredientSO ingredient)
    {
        savedIngredient = ingredient;
        if (ingredient != null)
        {
            image.sprite = ingredient.ingredientMiniSpr;
        }
    }

    public void SetActive(bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
    }
}
