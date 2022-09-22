using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CounterSelectButtonUI : MonoBehaviour
{
    private Button button;
    [SerializeField] TextMeshProUGUI buttonText;

    private Action onClick;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (onClick != null)
            {
                onClick.Invoke();
            }
            Global.Sound.Play("SFX/button_interactive00", Define.Sound.Effect);
        });
    }

    public void Init(string text, Action onClickEvent)
    {
        onClick = null;
        buttonText.text = text;
        onClick += onClickEvent;
    }
}
