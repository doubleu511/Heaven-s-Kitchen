using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : InteractiveObject
{
    public override void OnInteract()
    {
        CookingManager.Counter.SetScroll(true, false);
    }
}
