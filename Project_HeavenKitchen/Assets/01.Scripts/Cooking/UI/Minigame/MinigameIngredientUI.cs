using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinigameIngredientUI : MonoBehaviour, IPointerDownHandler
{
    private bool isClickAble = false;
    public Action clickAction;

    public void SetClickAble()
    {
        if(!isClickAble)
        {
            isClickAble = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isClickAble)
        {
            if (clickAction != null)
            {
                clickAction.Invoke();
            }
        }
    }
}
