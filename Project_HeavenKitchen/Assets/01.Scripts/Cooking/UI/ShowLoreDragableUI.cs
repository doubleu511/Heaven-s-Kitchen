using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowLoreDragableUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool isRequired = true;

    protected Image myImg;
    public IngredientSO myIngredient;
    public TabInfo myInfo;

    public void SetIngredient(IngredientSO ingredient)
    {
        myIngredient = ingredient;

        if (ingredient != null)
        {
            myImg.color = Color.white;
            myImg.sprite = ingredient.ingredientMiniSpr;
        }
        else
        {
            myImg.color = new Color(1, 1, 1, 0);
            myImg.sprite = null;
        }
    }

    public virtual void SetTabInfo(TabInfo info)
    {
        if (info != null)
        {
            myInfo = info;
        }
        else
        {
            myInfo = new TabInfo();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRequired)
        {
            CookingManager.Global.IngredientLore.SetNeed(myIngredient, myInfo);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CookingManager.Global.IngredientLore.HideLore();
    }
}
