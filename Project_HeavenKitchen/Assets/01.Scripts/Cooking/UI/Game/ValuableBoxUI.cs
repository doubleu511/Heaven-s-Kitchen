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
        if(textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
        }

        textCoroutine = StartCoroutine(TextCoroutine(current, target));

        if(valuableTextBoxTrm != null)
        {
            ValuableEffectUI effectUI = Global.Pool.GetItem<ValuableEffectUI>();

        }
    }

    private IEnumerator TextCoroutine(int current, int target)
    {
        float fCurrent = current;
        float fTarget = target;

        valuableText.text = $"{current}";

        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        if (current < target)
        {
            while (fCurrent < fTarget)
            {
                fCurrent += offset * Time.deltaTime;
                valuableText.text = ((int)fCurrent).ToString();
                yield return null;
            }
        }
        else if (current > target)
        {
            while (fCurrent > fTarget)
            {
                fCurrent += offset * Time.deltaTime;
                valuableText.text = ((int)fCurrent).ToString();
                yield return null;
            }
        }

        valuableText.text = $"{target}";
    }
}
