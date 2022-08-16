using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceCooker : IngredientSender
{
    private SpriteRenderer riceCookerBaseSR;
    public SpriteRenderer riceCookerTopSR;

    public Sprite spr_riceCookerBase_Closed;
    public Sprite spr_riceCookerBase_Opened;

    private void Awake()
    {
        riceCookerBaseSR = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract()
    {
        CookingManager.Player.Inventory.InventoryAdd(ingredientBox);

        riceCookerBaseSR.sprite = spr_riceCookerBase_Opened;
        riceCookerTopSR.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            riceCookerBaseSR.sprite = spr_riceCookerBase_Closed;
            riceCookerTopSR.gameObject.SetActive(false);
        }
    }
}