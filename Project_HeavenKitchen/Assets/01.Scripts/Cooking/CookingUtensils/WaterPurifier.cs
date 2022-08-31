using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifier : IngredientSender
{
    public override void OnInteract()
    {
        CookingManager.Player.Inventory.InventoryAdd(ingredientBox);
    }
}