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
    [SerializeField] TextMeshProUGUI dialogText = null;
    [SerializeField] Text phoneDialogText = null;
    [SerializeField] GameObject textEndArrow;

    private bool isPlayingDialog = false; // 현재 하나의 문단 다이얼로그가 재생중인가?
    private bool isText = false;
    private bool isTextEnd = false;

    public static bool isClicked = false;

    private Queue<CookingDialogInfo> dialogQueue = new Queue<CookingDialogInfo>();

    private Coroutine textCoroutine = null;
    private Tweener textTween = null;

    private string textString = "";

    private void Awake()
    {
        dialogPanel = GetComponent<CanvasGroup>();
        dialogEvents = GetComponent<CookingDialogEvents>();
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

            textCoroutine = StartCoroutine(TextCoroutine());
        }
    }

    private IEnumerator TextCoroutine()
    {
        while (dialogQueue.Count > 0)
        {
            isClicked = false;
            CookingDialogInfo dialog = dialogQueue.Dequeue();
            ShowText(TranslationManager.Instance.GetLangDialog(dialog.tranlationId));

            yield return new WaitUntil(() => isTextEnd);
            // 이벤트가 걸리는지 확인
            EventTest(dialog);
            yield return new WaitUntil(() => !eventWaitFlag);

            yield return new WaitUntil(() => isClicked);
        }

        Global.UI.UIFade(dialogPanel, false);
        isPlayingDialog = false;
    }

    private void EventTest(CookingDialogInfo info)
    {
        if (false == string.IsNullOrEmpty(info.eventMethod))
        {
            eventWaitFlag = true;
            dialogEvents.ThrowEvent(info.eventMethod);
        }
    }

    public void SetCharacterFace(Image chara, CharacterSO charaData, int faceIndex)
    {
        Image faceTrm = chara.transform.Find("FaceImg").GetComponent<Image>();

        faceTrm.sprite = charaData.faceBox[faceIndex];
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

                        if (parsed_textString != parsed_dialogText)
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
