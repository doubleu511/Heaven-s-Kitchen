using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GuestTalkInKitchenUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] Image characterPortrait;

    [Header("SpeechBubbles")]
    [SerializeField] CanvasGroup[] speechBubbles;
    [SerializeField] TextMeshProUGUI[] speechBubbleTexts;
    private int curIndex = 0;

    private bool isBubblePlaying = false;
    private Queue<string> bubbleQueue = new Queue<string>();
    private Coroutine bubbleCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowGuestTalk(Sprite sprite)
    {
        SetPortrait(sprite);
        canvasGroup.alpha = 1;
    }

    public void HideGuestTalk()
    {
        canvasGroup.alpha = 0;
        HideSpeechBubbles();
    }

    public void SetPortrait(Sprite sprite)
    {
        characterPortrait.sprite = sprite;
    }

    public void HideSpeechBubbles()
    {
        for (int i = 0; i < speechBubbles.Length; i++)
        {
            speechBubbles[i].DOKill();
            speechBubbles[i].alpha = 0;
        }
    }

    public void AddBubbleMessage(string value)
    {
        bubbleQueue.Enqueue(value);

        if (bubbleCoroutine == null)
        {
            bubbleCoroutine = StartCoroutine(BubbleMessage());
        }
    }

    private IEnumerator BubbleMessage()
    {
        while(bubbleQueue.Count > 0)
        {
            string text = bubbleQueue.Dequeue();

            CanvasGroup oldOne = speechBubbles[curIndex];
            curIndex = (curIndex + 1) % 2;
            CanvasGroup newOne = speechBubbles[curIndex];

            speechBubbleTexts[curIndex].text = text;
            UtilClass.ForceRefreshSize(newOne.transform);

            SpeechBubbleChange(oldOne, newOne);

            yield return new WaitUntil(() => !isBubblePlaying);
            yield return new WaitForSeconds(1);
        }

        bubbleCoroutine = null;
    }

    public void SpeechBubbleChange(CanvasGroup oldOne, CanvasGroup newOne)
    {
        isBubblePlaying = true;

        RectTransform oldRect = oldOne.GetComponent<RectTransform>();
        RectTransform newRect = newOne.GetComponent<RectTransform>();

        newRect.anchoredPosition = new Vector3(newRect.anchoredPosition.x, -150);
        newRect.localScale = new Vector3(0, 0, 1);
        newOne.alpha = 0;

        oldRect.DOAnchorPosY(0, 1).SetEase(Ease.Linear);
        newOne.DOFade(1, 1);
        newRect.DOScale(Vector3.one, 1).OnComplete(() => isBubblePlaying = false);
    }
}