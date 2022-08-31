using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterPanel : MonoBehaviour
{
    [SerializeField] Button GoToCookingBtn;

    private RectTransform scrollRect;

    private void Awake()
    {
        scrollRect = GetComponent<RectTransform>();

        GoToCookingBtn.onClick.AddListener(() =>
        {
            SetScroll(false, false);
        });
    }

    public void SetScroll(bool value, bool IsImmediately)
    {
        if(value)
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(0, 0);
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(0, 0), 1).SetEase(Ease.OutBounce);
            }
        }
        else
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(1920, 0);
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(1920, 0), 1).SetEase(Ease.OutBounce);
            }
        }
    }
}
