using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteDragableUI : DragableUI
{
    public override void OnBeginDrag(PointerEventData eventData)
    { 
        
    }

    public override void OnDrag(PointerEventData eventData)
    {

    }

    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public override void OnDrop(PointerEventData eventData) // OnDrop�� OnEndDrag���� ���� ����ȴ�.
    {
        // TO DO : ���� �ִϸ��̼�

        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null);
        CookingManager.Global.DragAndDropContainer.gameObject.SetActive(false);
    }
}
