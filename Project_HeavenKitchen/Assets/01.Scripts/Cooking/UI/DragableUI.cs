using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 요리 미니게임에서, 인벤토리에 있는 재료를 드래그할때 사용합니다.
/// </summary>
public class DragableUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
{
    protected Image myImg;
    public IngredientSO myIngredient;
    protected bool isDragging = false;

    private void Awake()
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

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled || myImg.sprite == null)
        {
            return;
        }

        // Activate Container
        CookingManager.Global.DragAndDropContainer.gameObject.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient);
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
            }

            SetIngredient(myIngredient);
        }

        myImg.enabled = true;
        isDragging = false;
        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null);
        CookingManager.Global.DragAndDropContainer.gameObject.SetActive(false);
    }

    public virtual void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {
        if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
        {
            // set data from drag object on Container
            SetIngredient(CookingManager.Global.DragAndDropContainer.savedIngredient);
            CookingManager.Global.DragAndDropContainer.SetIngredient(null);
            CookingManager.Global.DragAndDropContainer.gameObject.SetActive(false);
        }
    }
}
