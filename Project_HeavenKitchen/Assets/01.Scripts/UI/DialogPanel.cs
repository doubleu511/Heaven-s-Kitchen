using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum WhereIsTalk
{
    Left = 1,
    Right = 2,
}

public enum CharacterSpeaker
{
    NONE = 0,
    OLIVE = 1,

}

public class DialogPanel : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup dialogPanel;

    [SerializeField] Text dialogText = null;
    [SerializeField] GameObject textEndArrow;
    [SerializeField] Button skipButton;

    [SerializeField] RectTransform leftTrm;
    [SerializeField] RectTransform rightTrm;

    [SerializeField] Animator[] characterAnims;

    private Dictionary<CharacterSpeaker, Animator> animDic = new Dictionary<CharacterSpeaker, Animator>();

    Animator leftChara;
    Animator rightChara;

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

        animDic.Add(CharacterSpeaker.NONE, null);
        animDic.Add(CharacterSpeaker.OLIVE, characterAnims[0]);
    }

    private void Start()
    {
        CanvasGroup btnSkipGroup = skipButton.GetComponent<CanvasGroup>();

        btnSkipGroup.DOFade(1, 0.5f).OnStart(() =>
        {
            btnSkipGroup.interactable = true;
            btnSkipGroup.blocksRaycasts = true;
        }).SetDelay(2).SetUpdate(true);
    }

    public void StartDialog(Dialog so)
    {
        if (!isPlayingDialog)
        {
            isPlayingDialog = true;
            Time.timeScale = 0;

            dialogPanel.blocksRaycasts = true;
            dialogPanel.interactable = true;

            dialogPanel.DOComplete();
            dialogPanel.DOFade(1, 0.15f).SetUpdate(true);

            textCoroutine = StartCoroutine(TextCoroutine(so));
        }
    }

    private IEnumerator TextCoroutine(Dialog so)
    {
        for (int i = 0; i < so.dialogInfos.Count; i++)
        {
            isFinished = false;
            ShowText(TranslationManager.Instance.GetLangDialog(so.dialogInfos[i].tranlationId));
            SettingCharacterTalk(so, i);
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

        leftChara = animDic[(CharacterSpeaker)dialog.dialogInfos[index].leftChracter];
        rightChara = animDic[(CharacterSpeaker)dialog.dialogInfos[index].rightChracter];

        if (leftChara)
        {
            leftChara.transform.localScale = new Vector2(-4, 4);
            leftChara.transform.position = leftTrm.transform.position;
            leftChara.gameObject.SetActive(true);

            leftChara.GetComponent<Image>().color = dialog.dialogInfos[index].type == (int)WhereIsTalk.Left ? Color.white : Color.gray;
        }

        if (rightChara)
        {
            rightChara.transform.localScale = new Vector2(4, 4);
            rightChara.transform.position = rightTrm.transform.position;
            rightChara.gameObject.SetActive(true);

            rightChara.GetComponent<Image>().color = dialog.dialogInfos[index].type == (int)WhereIsTalk.Right ? Color.white : Color.gray;
        }
    }

    private void ShowText(string text)
    {
        isText = true;
        isTextEnd = false;

        dialogText.text = "";
        textEndArrow.SetActive(false);
        int textLength = TextLength(text);

        textTween = dialogText.DOText(text, textLength * 0.1f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        textString = "";
                        isTextEnd = true;
                        textEndArrow.SetActive(true);
                        if (leftChara)
                        {
                            leftChara.SetBool("isTalk", false);
                        }

                        if (rightChara)
                        {
                            rightChara.SetBool("isTalk", false);
                        }
                    })
                    .OnUpdate(() =>
                    {
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
