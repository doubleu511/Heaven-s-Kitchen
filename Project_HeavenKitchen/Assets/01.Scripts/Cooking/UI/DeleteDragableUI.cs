using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteDragableUI : DragableUI
{
    public Action<IngredientSO> onDelete;

    public override void OnBeginDrag(PointerEventData eventData)
    { 
        
    }

    public override void OnDrag(PointerEventData eventData)
    {

    }

    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public override void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {
        if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
        {
            // TO DO : 삭제 애니메이션
            if (onDelete != null)
            {
                onDelete.Invoke(CookingManager.Global.DragAndDropContainer.savedIngredient);
            }

            // Reset Contatiner
            CookingManager.Global.DragAndDropContainer.SetIngredient(null);
            CookingManager.Global.DragAndDropContainer.gameObject.SetActive(false);
        }
    }
}
