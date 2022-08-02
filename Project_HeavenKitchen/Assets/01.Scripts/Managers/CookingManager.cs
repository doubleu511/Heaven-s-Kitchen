using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static PlayerController Player;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>(true);
    }
}
