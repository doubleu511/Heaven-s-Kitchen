using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

using static Define;

public class DialogPanel : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup dialogPanel;
    private DialogEvents dialogEvents;
    public static bool eventWaitFlag = false;

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

    private Dictionary<CharacterSpeaker, CharacterSO> charaDataDic = new Dictionary<CharacterSpeaker, CharacterSO>();
    private Dictionary<CharacterSpeaker, Image> charaImageDic = new Dictionary<CharacterSpeaker, Image>();
    private Dictionary<CharacterTextStyle, TextMeshProUGUI> charaTextStyleDic = new Dictionary<CharacterTextStyle, TextMeshProUGUI>();

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
        dialogEvents = GetComponent<DialogEvents>();

        skipButton.onClick.AddListener(() =>
        {
            DialogSkip();
        });

        logButton.onClick.AddListener(() => Global.UI.UIFade(logCanvasGroup, true));

        logCancelBtn.onClick.AddListener(() => Global.UI.UIFade(logCanvasGroup, false));

        for (int i = 0; i < characterImages.Length; i++)
        {
            charaDataDic[(CharacterSpeaker)i] = characterDatas[i];
            charaImageDic[(CharacterSpeaker)i] = characterImages[i];
        }

        for (int i = 0; i < characterTextStyles.Length; i++)
        {
            charaTextStyleDic[(CharacterTextStyle)i] = characterTextStyles[i];
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

            Global.UI.UIFade(dialogPanel, UIFadeType.IN, 0.15f, true);
            Global.UI.UIFade(topUICanvasGroup, UIFadeType.IN, 0.5f, true);

            textCoroutine = StartCoroutine(TextCoroutine(dialog));
        }
    }

    private IEnumerator TextCoroutine(Dialog dialog)
    {
        for (int i = 0; i < dialog.dialogInfos.Count; i++)
        {
            isFinished = false;
            // 이벤트가 걸리는지 확인
            EventTest(dialog.dialogInfos[i]);
            if (eventWaitFlag) i++;
            if (i >= dialog.dialogInfos.Count) break;
            yield return new WaitUntil(() => !eventWaitFlag);
            ShowText(TranslationManager.Instance.GetLangDialog(dialog.dialogInfos[i].tranlationId));
            SetBackground(backgrounds[dialog.dialogInfos[i].background]);
            SettingCharacterTalk(dialog, i);
            yield return new WaitUntil(() => isFinished);
        }

        Global.UI.UIFade(dialogPanel, UIFadeType.OUT, 0.5f, true);
        isPlayingDialog = false;
        Time.timeScale = 1;
    }

    private void EventTest(DialogInfo info)
    {
        if(info.type == (int)DialogType.ACTIONEVENT)
        {
            eventWaitFlag = true;
            dialogEvents.ThrowEvent(info.eventName);
        }
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

        leftChara = charaImageDic[(CharacterSpeaker)dialog.dialogInfos[index].leftChracter];
        rightChara = charaImageDic[(CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        CharacterSO currentCharaData =
            dialog.dialogInfos[index].type == (int)DialogType.TALKLEFT ?
            charaDataDic[(CharacterSpeaker)dialog.dialogInfos[index].leftChracter] :
            charaDataDic[(CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        dialogText.font = currentCharaData.characterFont;

        UI_DialogSpeechBubble speechBubble = Global.Pool.GetItem<UI_DialogSpeechBubble>();
        speechBubble.InitBubble(currentCharaData, (DialogType)dialog.dialogInfos[index].type,
    TranslationManager.Instance.GetLangDialog(dialog.dialogInfos[index].tranlationId));

        CharacterInit(leftChara, DialogType.TALKLEFT);
        CharacterInit(rightChara, DialogType.TALKRIGHT);

        void CharacterInit(Image chara, DialogType currentWhereTalk)
        {
            if (chara)
            {
                if (currentWhereTalk == DialogType.TALKLEFT)
                {
                    chara.transform.localScale = new Vector2(-1, 1);
                    chara.transform.position = leftTrm.transform.position;
                }
                else if (currentWhereTalk == DialogType.TALKRIGHT)
                {
                    chara.transform.localScale = new Vector2(1, 1);
                    chara.transform.position = rightTrm.transform.position;
                }

                Vector3 arrowPos = dialog.dialogInfos[index].type == (int)DialogType.TALKLEFT ?
                    leftTrm.transform.position :
                    rightTrm.transform.position;
                speechBubbleArrow.transform.position = new Vector3(arrowPos.x, speechBubbleArrow.transform.position.y);

                chara.gameObject.SetActive(true);

                if (dialog.dialogInfos[index].type == (int)currentWhereTalk)
                {
                    SetCharacterColor(chara, Color.white);
                    SetTextStyle(currentCharaData.textStyle);
                    SetCharacterFace(chara, currentCharaData, dialog.dialogInfos[index].faceIndex);

                    int clothesIndex = currentWhereTalk == DialogType.TALKLEFT ?
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

    public void SetTextStyle(CharacterTextStyle textStyle)
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
                            Global.Sound.Play("SFX/talk", Sound.Effect);
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

            Global.UI.UIFade(dialogPanel, UIFadeType.OUT, 0.5f, true);
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
