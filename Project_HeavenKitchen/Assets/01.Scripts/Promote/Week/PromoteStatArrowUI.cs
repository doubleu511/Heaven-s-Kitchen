using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PromoteStatArrowUI : MonoBehaviour
{
    [SerializeField] Image arrowImg;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    public void Init(int delta, Define.StatType statType)
    {
        if(delta == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        arrowImg.color = new Color(arrowImg.color.r, arrowImg.color.g, arrowImg.color.b, 0);
        arrowImg.transform.position = transform.position;

        arrowImg.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        arrowImg.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutBack);
        arrowImg.transform.DOScaleY(1, 0.5f).SetEase(Ease.InOutBack);
        arrowImg.DOFade(1, 0.5f);

        if (delta > 0)
        {
            arrowImg.sprite = statType == Define.StatType.STRESS ? PromoteManager.Promote.statArrowBlueUp : PromoteManager.Promote.statArrowRedUp;
            arrowImg.transform.DOMove(top.position, 1f);
        }
        else
        {
            arrowImg.sprite = statType == Define.StatType.STRESS ? PromoteManager.Promote.statArrowRedDown : PromoteManager.Promote.statArrowBlueDown;
            arrowImg.transform.DOMove(bottom.position, 1f);
        }

        arrowImg.DOFade(0, 1).SetDelay(2);
    }
}
