using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterDishUI : MonoBehaviour
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    [SerializeField] Image dishImg;
    [SerializeField] Image foodImg;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(IngredientSO ingredient)
    {
        Global.UI.UIFade(canvasGroup, true);
        foodImg.sprite = ingredient.ingredientDefaulrSpr;

        rect.anchoredPosition = new Vector2(0, -25);
        rect.DOAnchorPos(new Vector2(0, 158), 1.5f).OnComplete(() =>
        {
            Global.UI.UIFade(canvasGroup, Define.UIFadeType.OUT, 0.5f, false, () =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
