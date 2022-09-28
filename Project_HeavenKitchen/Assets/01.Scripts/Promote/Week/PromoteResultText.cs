using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PromoteResultText : MonoBehaviour
{
    [SerializeField] CanvasGroup textBg;
    [SerializeField] CanvasGroup startText;
    [SerializeField] CanvasGroup successText;
    [SerializeField] CanvasGroup failText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowStart();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowSuccess();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            ShowFail();
        }
    }

    public void ShowStart()
    {
        InvokeAppear(startText);

        startText.transform.localScale = new Vector3(0.5f, 0.2f, 1);

        startText.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutBack);
        startText.transform.DOScaleY(1, 0.75f).SetEase(Ease.InOutBack);
    }

    public void ShowSuccess()
    {
        InvokeAppear(successText);
        ShowBackground();

        successText.gameObject.SetActive(true);
        failText.gameObject.SetActive(false);

        successText.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        successText.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutBack);
        successText.transform.DOScaleY(1, 0.5f).SetEase(Ease.InOutBack);
    }

    public void ShowFail()
    {
        InvokeAppear(failText);
        ShowBackground();

        failText.gameObject.SetActive(true);
        successText.gameObject.SetActive(false);

        failText.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        failText.transform.DOScaleX(1, 0.5f).SetEase(Ease.OutBack);
        failText.transform.DOScaleY(1, 0.5f).SetEase(Ease.InOutBack);
    }

    private void ShowBackground()
    {
        InvokeAppear(textBg);
        textBg.transform.localScale = new Vector3(0, 1, 1);
        textBg.transform.DOScaleX(1, 0.2f);
    }

    private void InvokeAppear(CanvasGroup group)
    {
        group.transform.DOKill();
        group.DOKill();
        group.alpha = 1;

        group.DOFade(0, 0.75f).SetDelay(1.5f);
    }
}
