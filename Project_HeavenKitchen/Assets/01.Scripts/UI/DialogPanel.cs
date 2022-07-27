using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class DialogPanel : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup dialogPanel;

    [SerializeField] TextMeshProUGUI dialogText = null;
    [SerializeField] Text phoneDialogText = null;
    [SerializeField] GameObject textEndArrow;
    [SerializeField] Button skipButton;

    [SerializeField] RectTransform leftTrm;
    [SerializeField] RectTransform rightTrm;

    [SerializeField] CharacterSO[] characterDatas;
    [SerializeField] Image[] characterImages;
    [SerializeField] TextMeshProUGUI[] characterTextStyles;

    private Dictionary<Define.CharacterSpeaker, CharacterSO> charaDataDic = new Dictionary<Define.CharacterSpeaker, CharacterSO>();
    private Dictionary<Define.CharacterSpeaker, Image> charaImageDic = new Dictionary<Define.CharacterSpeaker, Image>();
    private Dictionary<Define.CharacterTextStyle, TextMeshProUGUI> charaTextStyleDic = new Dictionary<Define.CharacterTextStyle, TextMeshProUGUI>();

    Image leftChara;
    Image rightChara;
    TextMeshProUGUI currentNameTextStyles;

    private bool isPlayingDialog = false; // 현재 하나의 문단 다이얼로그가 재생중인가?
    private bool isText = false;
    private bool isTextEnd = false;
    private bool isFinished = false;

    private Coroutine textCoroutine = null;
    private Tweener textTween = null;

    private string textString = "";

    private void Awake()
    {
        dialogPanel = GetComponent<CanvasGroup>();

        for (int i = 0; i < characterImages.Length; i++)
        {
            charaDataDic[(Define.CharacterSpeaker)i] = characterDatas[i];
            charaImageDic[(Define.CharacterSpeaker)i] = characterImages[i];
        }

        for (int i = 0; i < characterTextStyles.Length; i++)
        {
            charaTextStyleDic[(Define.CharacterTextStyle)i] = characterTextStyles[i];
        }
    }

    private void Start()
    {
        CanvasGroup btnSkipGroup = skipButton.GetComponent<CanvasGroup>();

        btnSkipGroup.DOFade(1, 0.5f).OnStart(() =>
        {
            btnSkipGroup.interactable = true;
            btnSkipGroup.blocksRaycasts = true;
        }).SetDelay(2).SetUpdate(true);

        StartDialog(TranslationManager.Instance.Dialog.GetDialog(0));
    }

    public void StartDialog(Dialog dialog)
    {
        if (!isPlayingDialog)
        {
            isPlayingDialog = true;
            Time.timeScale = 0;

            dialogPanel.blocksRaycasts = true;
            dialogPanel.interactable = true;

            dialogPanel.DOComplete();
            dialogPanel.DOFade(1, 0.15f).SetUpdate(true);

            textCoroutine = StartCoroutine(TextCoroutine(dialog));
        }
    }

    private IEnumerator TextCoroutine(Dialog dialog)
    {
        for (int i = 0; i < dialog.dialogInfos.Count; i++)
        {
            isFinished = false;
            ShowText(TranslationManager.Instance.GetLangDialog(dialog.dialogInfos[i].tranlationId));
            SettingCharacterTalk(dialog, i);
            yield return new WaitUntil(() => isFinished);
        }

        dialogPanel.blocksRaycasts = false;
        dialogPanel.interactable = false;
        isPlayingDialog = false;
        Time.timeScale = 1;

        dialogPanel.DOFade(0, 0.5f).SetUpdate(true);
    }

    private void SettingCharacterTalk(Dialog dialog, int index)
    {
        if (leftChara)
        {
            leftChara.gameObject.SetActive(false);
        }

        if (rightChara)
        {
            rightChara.gameObject.SetActive(false);
        }

        leftChara = charaImageDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].leftChracter];
        CharacterSO leftCharaData = charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].leftChracter];

        rightChara = charaImageDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].rightChracter];
        CharacterSO rightCharaData = charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        if (leftChara)
        {
            leftChara.transform.localScale = new Vector2(-1, 1);
            leftChara.transform.position = leftTrm.transform.position;
            leftChara.gameObject.SetActive(true);

            if(dialog.dialogInfos[index].type == (int)Define.WhereIsTalk.Left)
            {
                leftChara.GetComponent<Image>().color = Color.white;
                SetTextStyle(leftCharaData.textStyle);
                currentNameTextStyles.text = TranslationManager.Instance.GetLangDialog(leftCharaData.characterNameTranslationId);
            }
            else
            {
                leftChara.GetComponent<Image>().color = Color.gray;
            }
        }

        if (rightChara)
        {
            rightChara.transform.localScale = new Vector2(1, 1);
            rightChara.transform.position = rightTrm.transform.position;
            rightChara.gameObject.SetActive(true);

            if (dialog.dialogInfos[index].type == (int)Define.WhereIsTalk.Right)
            {
                rightChara.GetComponent<Image>().color = Color.white;
                SetTextStyle(rightCharaData.textStyle);
                currentNameTextStyles.text = TranslationManager.Instance.GetLangDialog(rightCharaData.characterNameTranslationId);
            }
            else
            {
                rightChara.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    private void ShowText(string text)
    {
        isText = true;
        isTextEnd = false;

        dialogText.text = "";
        phoneDialogText.text = "";
        textEndArrow.SetActive(false);
        int textLength = TextLength(text);

        textTween = phoneDialogText.DOText(text, textLength * 0.1f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        textString = "";
                        isTextEnd = true;
                        textEndArrow.SetActive(true);
                    })
                    .OnUpdate(() =>
                    {
                        dialogText.text = phoneDialogText.text;
                        string parsed_textString = textString.Replace(" ", "");
                        string parsed_dialogText = dialogText.text.Replace(" ", "");

                        if(parsed_textString != parsed_dialogText)
                        {
                            
                        }

                        textString = dialogText.text;
                    }).SetUpdate(true);

        int TextLength(string richText)
        {
            int len = 0;
            bool inTag = false;

            foreach (var ch in richText)
            {
                if (ch == '<')
                {
                    inTag = true;
                    continue;
                }
                else if (ch == '>')
                {
                    inTag = false;
                }
                else if (!inTag)
                {
                    len++;
                }
            }

            return len;
        }
    }

    public void SetTextStyle(Define.CharacterTextStyle textStyle)
    {
        for (int i = 0; i < characterTextStyles.Length; i++)
        {
            characterTextStyles[i].gameObject.SetActive(false);
        }

        charaTextStyleDic[textStyle].gameObject.SetActive(true);
        currentNameTextStyles = charaTextStyleDic[textStyle];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isText) return;

        if (!isTextEnd)
        {
            isTextEnd = true;
            textTween.Complete();
        }
        else
        {
            isText = false;
            isFinished = true;
        }
    }

}
