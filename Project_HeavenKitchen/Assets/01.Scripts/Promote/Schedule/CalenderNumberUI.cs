using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CalenderNumberUI : MonoBehaviour
{
    [HideInInspector] public Image image;

    [SerializeField] RectTransform maskRect;
    [SerializeField] Image maskedImage;
    [SerializeField] Image dateMarker;

    private bool isMarked = false;
    
    public TextMeshProUGUI numberText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public Tween FillSprite(Sprite sprite, float duration)
    {
        maskedImage.DOComplete();

        maskedImage.sprite = sprite;
        maskedImage.color = isMarked ? new Color(1, 1, 1, 0.3f) : Color.white;

        return maskRect.DOSizeDelta(new Vector2(image.rectTransform.sizeDelta.x, maskRect.sizeDelta.y), duration).SetEase(Ease.Linear);
    }

    public void EmptySprite()
    {
        maskedImage.DOFade(0, 0.5f).OnComplete(() =>
        {
            maskRect.sizeDelta = new Vector2(0, maskRect.sizeDelta.y);
            maskedImage.sprite = null;
        });
    }

    public void SetMarker(Sprite markerSpr)
    {
        if(markerSpr)
        {
            dateMarker.sprite = markerSpr;
        }

        isMarked = markerSpr != null;
        dateMarker.gameObject.SetActive(isMarked);
    }
}