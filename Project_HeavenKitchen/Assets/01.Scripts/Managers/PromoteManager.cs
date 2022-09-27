using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteManager : MonoBehaviour
{
    public static PromoteManager Global;
    public static Calender Calender;

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        Calender = FindObjectOfType<Calender>();
    }
}
