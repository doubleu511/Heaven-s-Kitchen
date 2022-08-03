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
    private Image myImg;
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
            myImg.enabled = true;
            myImg.sprite = ingredient.ingredientMiniSpr;
        }
        else
        {
            myImg.enabled = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled)
        {
            return;
        }

        // Activate Container
        CookingManager.Global.DragAndDropContainer.gameObject.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            CookingManager.Global.DragAndDropContainer.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
            {
                // set data from dropped object
                myIngredient = CookingManager.Global.DragAndDropContainer.savedIngredient;
                SetIngredient(myIngredient);
            }
            else
            {
                myIngredient = null;
                // Clear Data
                SetIngredient(myIngredient);
            }
        }

        isDragging = false;
        // Reset Contatiner
        CookingManager.Global.DragAndDropContainer.SetIngredient(null);
        CookingManager.Global.DragAndDropContainer.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
