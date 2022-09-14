using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyDragableUI : DragableUI
{
    public override void OnBeginDrag(PointerEventData eventData)
    { 
        // Activate Container
        CookingManager.Global.DragAndDropContainer.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient, null);
        isDragging = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null, null);
        CookingManager.Global.DragAndDropContainer.SetActive(false);
    }

    public override void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {

    }
}
