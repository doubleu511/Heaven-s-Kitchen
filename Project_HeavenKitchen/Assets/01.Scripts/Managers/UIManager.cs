using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager
{
    public void UIFade(CanvasGroup group, bool fade, float duration, bool setUpdate, UnityAction callback = null)
    {
        if (fade)
        {
            group.DOFade(1, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                group.interactable = true;
                group.blocksRaycasts = true;

                if (callback != null)
                    callback.Invoke();
            });
        }
        else
        {
            group.interactable = false;
            group.blocksRaycasts = false;

            group.DOFade(0, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                if (callback != null)
                    callback.Invoke();
            });
        }
    }

    public void UIFade(CanvasGroup group, bool fade)
    {
        group.alpha = fade ? 1 : 0;
        group.blocksRaycasts = fade;
        group.interactable = fade;
    }
}
