using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifier : IngredientSender
{
    [SerializeField] ParticleSystem waterSmokeParticle;

    public override void OnInteract()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(ingredientBox))
        {
            waterSmokeParticle.Play();
        }
    }
}