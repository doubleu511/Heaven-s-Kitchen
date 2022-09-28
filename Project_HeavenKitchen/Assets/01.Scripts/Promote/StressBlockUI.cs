using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StressBlockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Image sizeImage;
    [SerializeField] Transform arrowTrm;

    private int savedValue = 0;
    private float savedSize = 0f;

    private void Start()
    {
        savedSize = sizeImage.rectTransform.sizeDelta.y;

        StatHandler.onStatChanged += OnStatChanged;
        OnStatChanged(Define.StatType.STRESS, false);
    }

    private void OnStatChanged(Define.StatType stat, bool animation)
    {
        if (stat == Define.StatType.STRESS)
        {
            float scale = StatHandler.statDic[Define.StatType.STRESS] / (float)StatHandler.stressMax;

            if (animation)
            {
                StartCoroutine(UtilClass.TextAnimationCoroutine(valueText, savedValue, StatHandler.statDic[Define.StatType.STRESS]));
                sizeImage.rectTransform.DOSizeDelta(new Vector2(sizeImage.rectTransform.sizeDelta.x, savedSize * scale), 0.5f);

                if (arrowTrm != null)
                {
                    PromoteStatArrowUI arrowUI = Global.Pool.GetItem<PromoteStatArrowUI>();
                    arrowUI.transform.SetParent(arrowTrm, false);
                    arrowUI.transform.position = arrowTrm.position;
                    arrowUI.Init(StatHandler.statDic[Define.StatType.STRESS] - savedValue, stat);
                }
            }
            else
            {
                valueText.text = StatHandler.statDic[Define.StatType.STRESS].ToString();
                sizeImage.rectTransform.sizeDelta = new Vector2(sizeImage.rectTransform.sizeDelta.x, savedSize * scale);
            }

            savedValue = StatHandler.statDic[Define.StatType.STRESS];

            if (scale >= 0.76f)
            {
                sizeImage.sprite = PromoteManager.Promote.stressRedSpr;
            }
            else if (scale >= 0.51f)
            {
                sizeImage.sprite = PromoteManager.Promote.stressYellowSpr;
            }
            else
            {
                sizeImage.sprite = PromoteManager.Promote.stressGreenSpr;
            }
        }
    }
}
