using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

using static Define;

public class CookingDialogPanel : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup dialogPanel;
    private CookingDialogEvents dialogEvents;
    public static bool eventWaitFlag = false;

    [Header("Dialog")]
    [SerializeField] CanvasGroup speechBubble = null;
    [SerializeField] TextMeshProUGUI guestNameText = null;
    [SerializeField] TextMeshProUGUI dialogText = null;
    private WordWobble dialogTextWobble;

    [SerializeField] TextMeshProUGUI textSizeFitter = null;
    [SerializeField] Text phoneDialogText = null;
    [SerializeField] Image textEndArrow;
    public static CookingDialogInfo currentDialog;

    [Header("Guest")]
    [SerializeField] Image guestPortrait;
    private GuestSO currentGuest;

    private bool isPlayingDialog = false; // 현재 하나의 문단 다이얼로그가 재생중인가?
    private bool isText = false;
    private bool isTextEnd = false;

    private bool isClicked = false;

    private Queue<CookingDialogInfo> dialogQueue = new Queue<CookingDialogInfo>();

    private Coroutine textCoroutine = null;
    private Tweener textTween = null;

    private string textString = "";

    private void Awake()
    {
        dialogPanel = GetComponent<CanvasGroup>();
        dialogEvents = GetComponent<CookingDialogEvents>();
        dialogTextWobble = dialogText.GetComponent<WordWobble>();
    }

    public void StartDialog(CookingDialog dialog)
    {
        for (int i = 0; i < dialog.dialogInfos.Count; i++)
        {
            dialogQueue.Enqueue(dialog.dialogInfos[i]);
        }

        if (!isPlayingDialog)
        {
            isPlayingDialog = true;

            Global.UI.UIFade(dialogPanel, true);
            ShowSpeechBubble(true);

            textCoroutine = StartCoroutine(TextCoroutine());
        }
    }

    public void ShowSpeechBubble(bool value)
    {
        Global.UI.UIFade(speechBubble, value);
    }

    private IEnumerator TextCoroutine()
    {
        while (dialogQueue.Count > 0)
        {
            isClicked = false;
            CookingDialogInfo dialog = dialogQueue.Dequeue();
            currentDialog = dialog;
            dialogTextWobble.StopWobble();
            ShowText(TranslationManager.Instance.GetLangDialog(dialog.tranlationId), (TextAnimationType)dialog.text_animation_type);

            textEndArrow.gameObject.SetActive(string.IsNullOrEmpty(dialog.eventMethod));
            textEndArrow.color = new Color(1, 1, 1, 0);
            Global.UI.UIFade(dialogPanel, true);

            // 이벤트가 걸리는지 확인
            if (EventTest(dialog))
            {
                yield return new WaitUntil(() => isTextEnd);
                dialogEvents.OnTextEnd();
                Global.UI.UIFade(dialogPanel, false);
                yield return new WaitUntil(() => !eventWaitFlag);
            }
            else
            {
                yield return new WaitUntil(() => isTextEnd);
                textEndArrow.color = Color.white;
                yield return new WaitUntil(() => isClicked);
            }
        }

        SetQuitMessage(currentGuest.quitTranslationIds);
        Global.UI.UIFade(dialogPanel, false);
        isPlayingDialog = false;
    }

    private bool EventTest(CookingDialogInfo info)
    {
        if (false == string.IsNullOrEmpty(info.eventMethod))
        {
            eventWaitFlag = true;
            dialogEvents.ThrowEvent(info.eventMethod);
            return true;
        }

        return false;
    }

    public void GuestInit(GuestSO guest)
    {
        currentGuest = guest;
        guestPortrait.sprite = currentGuest.guestPortrait;

        int randomName = Random.Range(0, currentGuest.guestNameTranslationIds.Length);
        guestNameText.text = TranslationManager.Instance.GetLangDialog(currentGuest.guestNameTranslationIds[randomName]);
    }

    public void SetCharacterFace(Image chara, CharacterSO charaData, int faceIndex)
    {
        Image faceTrm = chara.transform.Find("FaceImg").GetComponent<Image>();

        faceTrm.sprite = charaData.faceBox[faceIndex];
    }

    public void SetQuitMessage(int[] translationId)
    {
        int random = Random.Range(0, translationId.Length);

        currentDialog = new CookingDialogInfo(translationId[random]);

        dialogText.text = TranslationManager.Instance.GetLangDialog(currentDialog.tranlationId);
        textSizeFitter.text = dialogText.text;
    }

    private void ShowText(string text, TextAnimationType textAnimation)
    {
        isText = true;
        isTextEnd = false;

        textSizeFitter.text = text;
        dialogText.text = "";
        phoneDialogText.text = "";
        int textLength = TextLength(text);

        if (textAnimation == TextAnimationType.SHAKE)
        {
            dialogTextWobble.SetWobble(text);
        }

        textTween = phoneDialogText.DOText(text, textLength * 0.1f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        textString = "";
                        isTextEnd = true;
                    })
                    .OnUpdate(() =>
                    {
                        dialogText.text = phoneDialogText.text;
                        string parsed_textString = textString.Replace(" ", "");
                        string parsed_dialogText = dialogText.text.Replace(" ", "");

                        if (parsed_textString != parsed_dialogText)
                        {
                            Global.Sound.Play("SFX/talk", Sound.Effect);
                        }

                        textString = dialogText.text;
                    });

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

    public void ResetEvent()
    {
        dialogEvents.ResetEvent();
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
            isClicked = true;
        }
    }
}
