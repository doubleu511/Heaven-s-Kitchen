using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UtensilsCircleInventoryUI : MonoBehaviour
{
    public enum BackgroundSpriteType
    {
        NORMAL,
        SPIKE
    }

    public enum StatusSpriteType
    {
        NONE,
        CHECK,
        WARNING,
        DEAD
    }

    public enum StatusAnimationType
    {
        NONE,
        FADE,
        BLINK,
        BLINK_FAST
    }

    [SerializeField] Image iconImg;
    [SerializeField] Image statusImg;

    private Image bgImg;
    private Coroutine animationCoroutine;
    private Tween animationTween;

    private void Awake()
    {
        bgImg = GetComponent<Image>();
    }

    public void Init(IngredientSO ingredient)
    {
        bgImg.enabled = true;
        SetStatusImage(null);
        iconImg.sprite = ingredient.ingredientMiniSpr;
    }

    public void SetBackgroundImage(Sprite spr)
    {
        if(spr == null)
        {
            bgImg.enabled = false;
        }
        else
        {
            bgImg.sprite = spr;
        }
    }

    public void SetStatusImage(Sprite spr)
    {
        if (spr == null)
        {
            statusImg.enabled = false;
        }
        else
        {
            statusImg.enabled = true;
            statusImg.sprite = spr;
        }
    }

    public void SetStatusAnimation(StatusAnimationType animationType)
    {
        if(animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        if(animationTween != null)
        {
            animationTween.Kill();
            animationTween = null;
        }
        statusImg.color = Color.white;

        switch (animationType)
        {
            case StatusAnimationType.NONE:
                break;
            case StatusAnimationType.FADE:
                animationTween = statusImg.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                break;
            case StatusAnimationType.BLINK:
                animationCoroutine = StartCoroutine(Blink(0.1f));
                break;
            case StatusAnimationType.BLINK_FAST:
                animationCoroutine = StartCoroutine(Blink(0.03f));
                break;
        }
    }

    public void SetPlus()
    {
        SetBackgroundImage(null);
        iconImg.sprite = UtensilsUIHandler.PlusSpr;
    }

    private IEnumerator Blink(float time)
    {
        while(true)
        {
            statusImg.color = new Color(1, 1, 1, 1);
            Global.Sound.Play("SFX/timertick", Define.Sound.Effect);
            yield return new WaitForSeconds(time);
            statusImg.color = new Color(1, 1, 1, 0.3f);
            yield return new WaitForSeconds(time);
        }
    }
}
