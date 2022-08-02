using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSender : InteractiveObject
{
    public IngredientSO ingredientBox;

    public override void OnInteract()
    {
        CookingManager.Player.Inventory.InventoryAdd(ingredientBox);
    }
}