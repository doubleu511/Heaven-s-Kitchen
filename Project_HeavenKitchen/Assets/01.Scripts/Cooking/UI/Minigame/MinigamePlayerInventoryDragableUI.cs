using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigamePlayerInventoryDragableUI : DragableUI
{
    protected Image dishImg;

    public override void Awake()
    {
        base.Awake();
        dishImg = transform.parent.Find("Dish").GetComponent<Image>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        dishImg.gameObject.SetActive(false);
    }

    public override void SetTabInfo(TabInfo info)
    {
        if (info != null)
        {
            myInfo = info;
        }
        else
        {
            myInfo = new TabInfo();
        }

        dishImg.gameObject.SetActive(myInfo.isDish);
    }
}
