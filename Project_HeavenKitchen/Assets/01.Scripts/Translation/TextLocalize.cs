using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalize : MonoBehaviour
{
    private Text text;
    public int translationId;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        TranslationManager.Instance.LocalizeChanged += CallBackLocalizeChanged;
        CallBackLocalizeChanged();
    }

    private void CallBackLocalizeChanged()
    {
        text.text = TranslationManager.Instance.GetLangDialog(translationId);
    }
}
