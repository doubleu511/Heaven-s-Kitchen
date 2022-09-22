using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterTimeBarUI : MonoBehaviour
{
    private RectTransform timerTotalRect;
    private CanvasGroup timerTotalCanvasGroup;
    [SerializeField] RectTransform barValue;
    [SerializeField] RectTransform[] barValueColors;

    [SerializeField] Image clockIconImg;
    [SerializeField] Image clockRedFill;

    [SerializeField] Transform clock;
    [SerializeField] Transform clockNeedle;

    private float initTimerSize = 1100f;
    private float initBarValue = 1080f;
    private bool hurryUp = false;

    private bool fade = false;

    private void Awake()
    {
        timerTotalRect = GetComponent<RectTransform>();
        timerTotalCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        initTimerSize = timerTotalRect.rect.width;
        initBarValue = barValue.rect.width;

        timerTotalRect.sizeDelta = new Vector2(0, timerTotalRect.sizeDelta.y);

        clockIconImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        clockIconImg.transform.DOLocalRotate(new Vector3(0, 0, 15f), 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetBarInterval(Vector3 interval)
    {
        // RED
        barValueColors[0].sizeDelta = new Vector2(GetSizeFromInitValue(interval.x), barValueColors[0].sizeDelta.y);

        // ORANGE
        barValueColors[1].anchoredPosition = new Vector2(GetSizeFromInitValue(interval.x), barValueColors[1].anchoredPosition.y);
        barValueColors[1].sizeDelta = new Vector2(GetSizeFromInitValue(interval.y - interval.x), barValueColors[1].sizeDelta.y);

        // YELLOW
        barValueColors[2].anchoredPosition = new Vector2(GetSizeFromInitValue(interval.y), barValueColors[2].anchoredPosition.y);
        barValueColors[2].sizeDelta = new Vector2(GetSizeFromInitValue(interval.z - interval.y), barValueColors[2].sizeDelta.y);

        // GREEN
        barValueColors[3].anchoredPosition = new Vector2(GetSizeFromInitValue(interval.z), barValueColors[3].anchoredPosition.y);
        barValueColors[3].sizeDelta = new Vector2(GetSizeFromInitValue(1 - interval.z), barValueColors[3].sizeDelta.y);
    }

    public void PlayAppearSeq()
    {
        if (fade)
        {
            timerTotalRect.DOSizeDelta(new Vector2(0, timerTotalRect.sizeDelta.y), 1).OnComplete(() =>
            {
                timerTotalCanvasGroup.DOFade(0, 1);
            });
        }
        else
        {
            timerTotalCanvasGroup.DOFade(1, 1).OnComplete(() =>
            {
                timerTotalRect.DOSizeDelta(new Vector2(initTimerSize, timerTotalRect.sizeDelta.y), 1);
            });
        }

        fade = !fade;
    }

    private float GetSizeFromInitValue(float scale)
    {
        return initBarValue * scale;
    }

    public void SetBarValue(float value)
    {
        float reverseValue = 1 - value;

        barValue.offsetMax = new Vector2(-initBarValue * reverseValue, barValue.offsetMax.y);
        clockRedFill.fillAmount = 1 - value;
        clockNeedle.localRotation = Quaternion.Euler(new Vector3(0, 0, (1 - value) * -360));

        if (!hurryUp)
        {
            if (value < CookingManager.Counter.TimerBarInterval.y)
            {
                hurryUp = true;
                StartCoroutine(HurryUp());
            }
        }

        if(hurryUp)
        {
            if (value >= CookingManager.Counter.TimerBarInterval.y)
            {
                hurryUp = false;
            }
        }
    }

    private IEnumerator HurryUp()
    {
        while(hurryUp)
        {
            yield return new WaitForSeconds(0.02f);
            clock.localRotation = Quaternion.Euler(new Vector3(0, 0, 5));
            yield return new WaitForSeconds(0.02f);
            clock.localRotation = Quaternion.Euler(new Vector3(0, 0, -5));
        }
    }
}
