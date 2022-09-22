using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] Define.StatType statType;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI valueText;

    private void Start()
    {
        StatHandler.onStatChanged += OnStatChanged;
        OnStatChanged();
    }

    private void OnStatChanged()
    {

    }
}
