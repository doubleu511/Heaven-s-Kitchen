using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeedyDragableUI : DragableUI
{
    [HideInInspector] public bool beginDragLock = false;
    public Action<bool> onPrepareItem;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled || myImg.sprite == null || beginDragLock)
        {
            return;
        }

        // Activate Container
        CookingManager.Global.DragAndDropContainer.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient);
        myImg.color = new Color(1, 1, 1, 0);
        myImg.sprite = null;
        myImg.enabled = false;
        isDragging = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            if (CookingManager.Global.DragAndDropContainer.savedIngredient == null)
            {
                beginDragLock = true;
                if (onPrepareItem != null)
                {
                    onPrepareItem.Invoke(false);
                }
            }

            SetIngredient(myIngredient);
        }

        myImg.enabled = true;
        isDragging = false;
        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null);
        CookingManager.Global.DragAndDropContainer.SetActive(false);
    }

    public override void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {
        if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
        {
            if (CookingManager.Global.DragAndDropContainer.savedIngredient != myIngredient)
                return;

            beginDragLock = false;
            if (onPrepareItem != null)
            {
                onPrepareItem.Invoke(true);
            }

            SetIngredient(CookingManager.Global.DragAndDropContainer.savedIngredient);
            CookingManager.Global.DragAndDropContainer.SetIngredient(null);
            CookingManager.Global.DragAndDropContainer.SetActive(false);
        }
    }
}
