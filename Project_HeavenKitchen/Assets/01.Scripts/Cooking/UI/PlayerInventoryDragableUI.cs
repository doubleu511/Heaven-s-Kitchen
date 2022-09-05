using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventoryDragableUI : DragableUI
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        List<IngredientSO> tempInventory = new List<IngredientSO>();
        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            if (CookingManager.Player.Inventory.inventoryTabs[i].ingredientInventoryUI.myIngredient != null)
            {
                tempInventory.Add(CookingManager.Player.Inventory.inventoryTabs[i].ingredientInventoryUI.myIngredient);
            }
        }

        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(null);

            if (i < tempInventory.Count)
            {
                CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(tempInventory[i]);
            }
        }
    }

    public override void OnDrop(PointerEventData eventData) // OnDrop이 OnEndDrag보다 먼저 실행된다.
    {
        
    }
}
