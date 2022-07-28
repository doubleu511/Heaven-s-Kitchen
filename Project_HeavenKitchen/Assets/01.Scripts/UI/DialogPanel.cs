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

    [Header("Dialog")]
    [SerializeField] Image backgroundImg = null;
    [SerializeField] TextMeshProUGUI dialogText = null;
    [SerializeField] Text phoneDialogText = null;
    [SerializeField] GameObject textEndArrow;
    [SerializeField] GameObject speechBubbleArrow;

    [Space(10)]
    [SerializeField] CanvasGroup topUICanvasGroup;
    [SerializeField] Button logButton;
    [SerializeField] Button skipButton;

    [Space(10)]
    [SerializeField] RectTransform leftTrm;
    [SerializeField] RectTransform rightTrm;

    [Space(10)]
    [SerializeField] Sprite[] backgrounds;
    [SerializeField] CharacterSO[] characterDatas;
    [SerializeField] Image[] characterImages;
    [SerializeField] TextMeshProUGUI[] characterTextStyles;

    [Header("Log")]
    [SerializeField] CanvasGroup logCanvasGroup;
    [SerializeField] Button logCancelBtn;
    [SerializeField] Transform scrollRectContentTrm;

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

        skipButton.onClick.AddListener(() =>
        {
            DialogSkip();
        });

        logButton.onClick.AddListener(() => Global.UI.UIFade(logCanvasGroup, true));

        logCancelBtn.onClick.AddListener(() => Global.UI.UIFade(logCanvasGroup, false));

        for (int i = 0; i < characterImages.Length; i++)
        {
            charaDataDic[(Define.CharacterSpeaker)i] = characterDatas[i];
            charaImageDic[(Define.CharacterSpeaker)i] = characterImages[i];
        }

        for (int i = 0; i < characterTextStyles.Length; i++)
        {
            charaTextStyleDic[(Define.CharacterTextStyle)i] = characterTextStyles[i];
        }

        GameObject speechBubblePrefab = Global.Resource.Load<GameObject>("UI/DialogSpeechBubble");
        Global.Pool.CreatePool<UI_DialogSpeechBubble>(speechBubblePrefab, scrollRectContentTrm, 10);
    }

    private void Start()
    {

        StartDialog(TranslationManager.Instance.Dialog.GetDialog(0));
    }

    public void StartDialog(Dialog dialog)
    {
        if (!isPlayingDialog)
        {
            isPlayingDialog = true;
            Time.timeScale = 0;

            Global.UI.UIFade(dialogPanel, true, 0.15f, true);
            Global.UI.UIFade(topUICanvasGroup, true, 0.5f, true);

            textCoroutine = StartCoroutine(TextCoroutine(dialog));
        }
    }

    private IEnumerator TextCoroutine(Dialog dialog)
    {
        for (int i = 0; i < dialog.dialogInfos.Count; i++)
        {
            isFinished = false;
            ShowText(TranslationManager.Instance.GetLangDialog(dialog.dialogInfos[i].tranlationId));
            SetBackground(backgrounds[dialog.dialogInfos[i].background]);
            SettingCharacterTalk(dialog, i);
            yield return new WaitUntil(() => isFinished);

            UI_DialogSpeechBubble speechBubble = Global.Pool.GetItem<UI_DialogSpeechBubble>();

            CharacterSO currentCharaData =
                dialog.dialogInfos[i].type == (int)Define.DialogType.TalkLeft ?
                charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[i].leftChracter] :
                charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[i].rightChracter];

            speechBubble.InitBubble(currentCharaData, (Define.DialogType)dialog.dialogInfos[i].type,
                TranslationManager.Instance.GetLangDialog(dialog.dialogInfos[i].tranlationId));
        }

        Global.UI.UIFade(dialogPanel, false, 0.5f, true);
        isPlayingDialog = false;
        Time.timeScale = 1;
    }

    public void SetBackground(Sprite background)
    {
        if(backgroundImg.sprite != background)
        backgroundImg.sprite = background;
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
        rightChara = charaImageDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        CharacterSO currentCharaData =
            dialog.dialogInfos[index].type == (int)Define.DialogType.TalkLeft ?
            charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].leftChracter] :
            charaDataDic[(Define.CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        CharacterInit(leftChara, Define.DialogType.TalkLeft);
        CharacterInit(rightChara, Define.DialogType.TalkRight);

        void CharacterInit(Image chara, Define.DialogType currentWhereTalk)
        {
            if (chara)
            {
                if (currentWhereTalk == Define.DialogType.TalkLeft)
                {
                    chara.transform.localScale = new Vector2(-1, 1);
                    chara.transform.position = leftTrm.transform.position;
                }
                else if (currentWhereTalk == Define.DialogType.TalkRight)
                {
                    chara.transform.localScale = new Vector2(1, 1);
                    chara.transform.position = rightTrm.transform.position;
                }

                Vector3 arrowPos = dialog.dialogInfos[index].type == (int)Define.DialogType.TalkLeft ?
                    leftTrm.transform.position :
                    rightTrm.transform.position;
                speechBubbleArrow.transform.position = new Vector3(arrowPos.x, speechBubbleArrow.transform.position.y);

                chara.gameObject.SetActive(true);

                if (dialog.dialogInfos[index].type == (int)currentWhereTalk)
                {
                    SetCharacterColor(chara, Color.white);
                    SetTextStyle(currentCharaData.textStyle);
                    SetCharacterFace(chara, currentCharaData, dialog.dialogInfos[index].faceIndex);

                    int clothesIndex = currentWhereTalk == Define.DialogType.TalkLeft ?
                                        dialog.dialogInfos[index].leftClothes :
                                        dialog.dialogInfos[index].rightClothes;
                    SetCharacterClothes(chara, currentCharaData, clothesIndex);
                    currentNameTextStyles.text = TranslationManager.Instance.GetLangDialog(currentCharaData.characterNameTranslationId);
                }
                else
                {
                    SetCharacterColor(chara, Color.gray);
                }
            }
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

    public void SetCharacterColor(Image chara, Color color)
    {
        Image[] charaImgs = chara.GetComponentsInChildren<Image>();

        for (int i = 0; i < charaImgs.Length; i++)
        {
            charaImgs[i].color = color;
        }
    }

    public void SetCharacterFace(Image chara, CharacterSO charaData, int faceIndex)
    {
        Image faceTrm = chara.transform.Find("FaceImg").GetComponent<Image>();

        faceTrm.sprite = charaData.faceBox[faceIndex];
    }

    public void SetCharacterClothes(Image chara, CharacterSO charaData, int clothesIndex)
    {
        if (chara.sprite != charaData.clothesBox[clothesIndex])
        {
            chara.sprite = charaData.clothesBox[clothesIndex];
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

    public void DialogSkip()
    {
        if (isPlayingDialog)
        {
            StopCoroutine(textCoroutine);
            textTween.Complete();

            textString = "";

            isPlayingDialog = false;

            Global.UI.UIFade(dialogPanel, false, 0.5f, true);
            Time.timeScale = 1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isText) return;
        if (logCanvasGroup.blocksRaycasts) return;

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
