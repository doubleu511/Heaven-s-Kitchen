using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : InteractiveObject
{
    public override void OnInteract()
    {
        CounterPanel counter = FindObjectOfType<CounterPanel>();

        counter.SetScroll(true, false);
    }
}
