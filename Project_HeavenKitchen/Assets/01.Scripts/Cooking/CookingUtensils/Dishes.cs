using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishes : InteractiveObject
{
    [SerializeField] DishUIHandler dishUI;

    public override void OnInteract()
    {
        dishUI.UIFade(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            dishUI.UIFade(false);
        }
    }
}
