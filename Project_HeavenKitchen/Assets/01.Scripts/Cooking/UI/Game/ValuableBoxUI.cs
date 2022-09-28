using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValuableBoxUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valuableText;
    [SerializeField] Transform valuableTextBoxTrm;

    public Define.MoneyType moneyType;

    private Coroutine textCoroutine;

    public void SetText(int current, int target)
    {
        if (current != target)
        {
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
                textCoroutine = null;
            }

            textCoroutine = StartCoroutine(UtilClass.TextAnimationCoroutine(valuableText, current, target));

            if (valuableTextBoxTrm != null)
            {
                ValuableEffectUI effectUI = Global.Pool.GetItem<ValuableEffectUI>();

                effectUI.transform.SetParent(valuableTextBoxTrm);
                effectUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

                effectUI.Init(moneyType, target - current);
            }
        }
    }

    public void SetText(int current, int target, int tipValue)
    {
        if (current != target)
        {
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
                textCoroutine = null;
            }

            textCoroutine = StartCoroutine(UtilClass.TextAnimationCoroutine(valuableText, current, target + tipValue));

            if (valuableTextBoxTrm != null)
            {
                ValuableEffectUI effectUI = Global.Pool.GetItem<ValuableEffectUI>();
                effectUI.transform.SetParent(valuableTextBoxTrm);
                effectUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                effectUI.Init(moneyType, target - current, false);

                ValuableEffectUI tipEffectUI = Global.Pool.GetItem<ValuableEffectUI>();
                tipEffectUI.transform.SetParent(valuableTextBoxTrm);
                tipEffectUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50, 0);
                tipEffectUI.Init(moneyType, tipValue, true);
            }
        }
    }

    public void SetText(int target)
    {
        valuableText.text = $"{target}";
    }
}
