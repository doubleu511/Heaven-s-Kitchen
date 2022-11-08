using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropContainer : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] Image ingredientImg;
    [SerializeField] Image dishImg;
    public IngredientSO savedIngredient;
    public TabInfo savedInfo;

    public Action<bool> onDrag;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetIngredient(IngredientSO ingredient, TabInfo info)
    {
        savedIngredient = ingredient;
        if (ingredient != null)
        {
            ingredientImg.sprite = ingredient.ingredientMiniSpr;

            if (info != null)
            {
                savedInfo = info;
            }
            else
            {
                savedInfo = new TabInfo();
            }

            // 특성에따른 상호작용
            {
                dishImg.gameObject.SetActive(savedInfo.isDish);
            }

            CookingManager.Global.IngredientLore.SetLore(savedIngredient, savedInfo);
        }
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            canvasGroup.alpha = 1;
            onDrag?.Invoke(true);
        }
        else
        {
            canvasGroup.alpha = 0;
            CookingManager.Global.IngredientLore.HideLore();
            onDrag?.Invoke(false);
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
