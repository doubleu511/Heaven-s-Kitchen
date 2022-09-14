using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabInfo
{
    public bool isDish;

    public TabInfo()
    {
        isDish = false;
    }
}

/// <summary>
/// �丮 �̴ϰ��ӿ���, �κ��丮�� �ִ� ��Ḧ �巡���Ҷ� ����մϴ�.
/// </summary>
public class DragableUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    protected Image myImg;
    public IngredientSO myIngredient;
    public TabInfo myInfo;

    protected bool isDragging = false;

    public virtual void Awake()
    {
        myImg = GetComponent<Image>();
    }

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

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled || myImg.sprite == null || myInfo.isDish)
        {
            return;
        }

        // Activate Container
        CookingManager.Global.DragAndDropContainer.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient, myInfo);
        myImg.color = new Color(1, 1, 1, 0);
        myImg.sprite = null;
        myImg.enabled = false;
        isDragging = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(eventData.position);
            world.z = 0;
            CookingManager.Global.DragAndDropContainer.transform.position = world;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            if (CookingManager.Global.DragAndDropContainer.savedIngredient == null)
            {
                myIngredient = null;
                myInfo = null;
            }

            SetIngredient(myIngredient);
            SetTabInfo(myInfo);
        }

        myImg.enabled = true;
        isDragging = false;
        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null, null);
        CookingManager.Global.DragAndDropContainer.SetActive(false);
    }

    public virtual void OnDrop(PointerEventData eventData) // OnDrop�� OnEndDrag���� ���� ����ȴ�.
    {
        if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
        {
            if (myIngredient == null)
            {
                // set data from drag object on Container
                SetIngredient(CookingManager.Global.DragAndDropContainer.savedIngredient);
                SetTabInfo(CookingManager.Global.DragAndDropContainer.savedInfo);
                CookingManager.Global.DragAndDropContainer.SetIngredient(null, null);
                CookingManager.Global.DragAndDropContainer.SetActive(false);
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (myIngredient != null)
        {
            if (myInfo != null)
            {
                if (myInfo.isDish)
                {
                    CookingManager.Global.IngredientLore.SetLore(myIngredient, myInfo);
                }
            }
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        CookingManager.Global.IngredientLore.HideLore();
    }
}
