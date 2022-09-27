using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PromoteStatDetailHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [SerializeField] Image detailImage;
    [SerializeField] TextMeshProUGUI nameText; 
    [SerializeField] TextMeshProUGUI loreText;
    [SerializeField] TextMeshProUGUI gainValueText;

    [Header("Cost")]
    [SerializeField] TextMeshProUGUI goldValueText;
    [SerializeField] GameObject stressCost;
    [SerializeField] TextMeshProUGUI stressValueText;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowInfo(int statTranslationId, PromoteStudy studyInfo)
    {
        Global.UI.UIFade(canvasGroup, Define.UIFadeType.IN, 0.5f, false);

        detailImage.sprite = studyInfo.studySprite;
        nameText.text = TranslationManager.Instance.GetLangDialog(studyInfo.studyTranslationId);
        loreText.text = TranslationManager.Instance.GetLangDialog(studyInfo.studyLoreTranslationId);

        if (studyInfo.gainStatValue > 0)
        {
            gainValueText.gameObject.SetActive(true);

            string valueText = "";
            valueText += $"{TranslationManager.Instance.GetLangDialog(statTranslationId)} + ";
            valueText += studyInfo.gainStatValue.ToString();
            gainValueText.text = valueText;
        }
        else
        {
            gainValueText.gameObject.SetActive(false);
        }

        goldValueText.text = $"{studyInfo.goldCost}G";

        if (studyInfo.stressValue > 0)
        {
            stressCost.SetActive(true);
            stressValueText.text = studyInfo.stressValue.ToString();
        }
        else
        {
            stressCost.SetActive(false);
        }

        UtilClass.ForceRefreshSize(transform);
    }

    public void HideInfo()
    {
        Global.UI.UIFade(canvasGroup, Define.UIFadeType.OUT, 0.5f, false);
    }
}
