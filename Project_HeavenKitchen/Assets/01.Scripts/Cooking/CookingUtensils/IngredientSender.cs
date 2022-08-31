using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSender : InteractiveObject
{
    public IngredientSO ingredientBox;

    public override void OnInteract()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(ingredientBox))
        {
            if (CookingManager.Global.MemoSuccessCountDic.ContainsKey(ingredientBox))
            {
                CookingManager.Global.MemoSuccessCountDic[ingredientBox]++;
            }
            else
            {
                CookingManager.Global.MemoSuccessCountDic[ingredientBox] = 1;
            }
        }
    }
}