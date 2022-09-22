using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalenderNumberUI : MonoBehaviour
{
    [HideInInspector] public Image image;

    public TextMeshProUGUI numberText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
}