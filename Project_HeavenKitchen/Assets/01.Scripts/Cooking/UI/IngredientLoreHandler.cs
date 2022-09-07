using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientLoreHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] RectTransform panelRect;
    [SerializeField] Image ingredientImg;

    [Header("LorePanel")]
    [SerializeField] CanvasGroup loreCanvasGroup;
    [SerializeField] TextMeshProUGUI ingredientName;
    [SerializeField] TextMeshProUGUI ingredientType;
    [SerializeField] TextMeshProUGUI ingredientLore;

    [Header("NeedPanel")]
    [SerializeField] CanvasGroup needCanvasGroup;
    [SerializeField] TextMeshProUGUI needIngredientName;

    [Header("Colors")]
    [SerializeField] Color ingredientColor;
    [SerializeField] Color foodColor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetLore(IngredientSO ingredient)
    {
        Global.UI.UIFade(needCanvasGroup, false);
        Global.UI.UIFade(loreCanvasGroup, true);

        ingredientImg.sprite = ingredient.ingredientMiniSpr;
        ingredientName.text = TranslationManager.Instance.GetLangDialog(ingredient.ingredientTranslationId);
        ingredientType.text = ingredient.isFood ? "음식" : "재료";
        ingredientType.color = ingredient.isFood ? foodColor : ingredientColor;
        ingredientLore.text = TranslationManager.Instance.GetLangDialog(ingredient.ingredientLoreId);

        // ContentSizeFitter를 강제 새로고침한다.
        ContentSizeFitter[] csfs = GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < csfs.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csfs[i].transform);
        }

        Global.UI.UIFade(canvasGroup, Define.UIFadeType.IN, 0.5f, false);

    }

    public void SetNeed(IngredientSO ingredient)
    {
        Global.UI.UIFade(needCanvasGroup, true);
        Global.UI.UIFade(loreCanvasGroup, false);

        ingredientImg.sprite = ingredient.ingredientMiniSpr;
        needIngredientName.text = TranslationManager.Instance.GetLangDialog(ingredient.ingredientTranslationId);
        needIngredientName.color = ingredient.isFood ? foodColor : ingredientColor;

        Global.UI.UIFade(canvasGroup, Define.UIFadeType.IN, 0.5f, false);
    }

    public void SetScreenPosToPos(Vector2 screenPos)
    {
        if(screenPos.x > 960)
        {
            panelRect.anchorMin = new Vector2(0, 0.5f);
            panelRect.anchorMax = new Vector2(0, 0.5f);
            panelRect.anchoredPosition = new Vector2(300, -21);
        }
        else
        {
            panelRect.anchorMin = new Vector2(1, 0.5f);
            panelRect.anchorMax = new Vector2(1, 0.5f);
            panelRect.anchoredPosition = new Vector2(-300, -21);
        }
    }

    public void HideLore()
    {
        Global.UI.UIFade(canvasGroup, Define.UIFadeType.OUT, 0.5f, false);
    }
}
