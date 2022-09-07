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
            CookingManager.Global.IngredientLore.SetLore(ingredient);
        }
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
            CookingManager.Global.IngredientLore.HideLore();
        }
    }

    private void Update()
    {
        if(savedIngredient != null)
        {
            CookingManager.Global.IngredientLore.SetScreenPosToPos(Input.mousePosition);
        }
    }
}
