using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValuableEffectUI : MonoBehaviour
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    [SerializeField] TextMeshProUGUI signText;
    [SerializeField] Image iconImg;
    [SerializeField] TextMeshProUGUI deltaText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Define.MoneyType moneyType, int delta)
    {
        bool isPlus = delta > 0;

        signText.text = isPlus ? "+" : "-";
        signText.color = CookingManager.Counter.plusMinusColors[isPlus ? 0 : 1];

        iconImg.sprite = CookingManager.Counter.moneyTypeSprs[(int)moneyType];

        deltaText.text = $"{Mathf.Abs(delta)}";
        if(moneyType == Define.MoneyType.GOLD)
        {
            deltaText.text += " $";
        }

        deltaText.color = CookingManager.Counter.plusMinusColors[isPlus ? 0 : 1];

        transform.localScale = new Vector3(0.8f, 0.8f, 1);
        transform.DOScale(1, 1.5f);

        rect.DOAnchorPosY(-50, 1f).SetRelative();

        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, 0.5f).SetDelay(1.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
