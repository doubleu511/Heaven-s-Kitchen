using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceCooker : IngredientSender
{
    private OutlineController outline;

    private SpriteRenderer riceCookerBaseSR;
    public SpriteRenderer riceCookerTopSR;

    public Sprite spr_riceCookerBase_Closed;
    public Sprite spr_riceCookerBase_Opened;

    private void Awake()
    {
        riceCookerBaseSR = GetComponent<SpriteRenderer>();
        outline = GetComponent<OutlineController>();
    }

    public override void OnInteract()
    {
        CookingManager.Player.Inventory.InventoryAdd(ingredientBox);

        riceCookerBaseSR.sprite = spr_riceCookerBase_Opened;
        outline.RefreshOutline();
        riceCookerTopSR.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            riceCookerBaseSR.sprite = spr_riceCookerBase_Closed;
            outline.RefreshOutline();
            riceCookerTopSR.gameObject.SetActive(false);
        }
    }
}