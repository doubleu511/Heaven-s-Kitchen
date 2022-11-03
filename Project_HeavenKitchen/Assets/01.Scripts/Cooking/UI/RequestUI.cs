using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestUI : MonoBehaviour, IDropHandler
{
    protected Image myImg;

    public Action<IngredientSO> onRequested;

    [HideInInspector] public bool cleanIngredientOnRequested = false;

    public virtual void Awake()
    {
        myImg = GetComponent<Image>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (CookingManager.Global.DragAndDropContainer.savedIngredient != null)
        {
            onRequested?.Invoke(CookingManager.Global.DragAndDropContainer.savedIngredient);
            if(cleanIngredientOnRequested)
            {
                CookingManager.Global.DragAndDropContainer.SetIngredient(null, null);
                CookingManager.Global.DragAndDropContainer.SetActive(false);
            }
        }
    }
}
