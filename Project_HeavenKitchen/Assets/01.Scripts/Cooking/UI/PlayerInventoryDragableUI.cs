using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventoryDragableUI : DragableUI
{
    protected Image dishImg;

    public override void Awake()
    {
        base.Awake();
        dishImg = transform.parent.Find("Dish").GetComponent<Image>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled || myImg.sprite == null)
        {
            return;
        }
        if (!CookingManager.Counter.IsInCounter && myInfo.isDish) return;

        // Activate Container
        CookingManager.Global.DragAndDropContainer.SetActive(true);
        // Set Data
        CookingManager.Global.DragAndDropContainer.SetIngredient(myIngredient, myInfo);
        myImg.color = new Color(1, 1, 1, 0);
        myImg.sprite = null;
        myImg.enabled = false;
        isDragging = true;
        dishImg.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        InventoryFill();
    }

    public override void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {
        base.OnDrop(eventData);
        InventoryFill();
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

    private void InventoryFill()
    {
        List<IngredientSO> tempInventory = new List<IngredientSO>();
        List<TabInfo> tempInventoryInfo = new List<TabInfo>();

        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            if (CookingManager.Player.Inventory.inventoryTabs[i].ingredientInventoryUI.myIngredient != null)
            {
                tempInventory.Add(CookingManager.Player.Inventory.inventoryTabs[i].ingredientInventoryUI.myIngredient);
                tempInventoryInfo.Add(CookingManager.Player.Inventory.inventoryTabs[i].tabinfo);
            }
        }

        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            if (i < tempInventory.Count)
            {
                CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(tempInventory[i]);
                CookingManager.Player.Inventory.inventoryTabs[i].SetInfo(tempInventoryInfo[i]);
            }
            else
            {
                CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(null);
                CookingManager.Player.Inventory.inventoryTabs[i].InitInfo();
            }
        }
    }
}
