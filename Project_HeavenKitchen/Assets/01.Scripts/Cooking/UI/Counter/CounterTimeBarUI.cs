using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterTimeBarUI : MonoBehaviour
{
    [SerializeField] RectTransform barValue;
    [SerializeField] RectTransform[] barValueColors;

    [SerializeField] Image clockIconImg;
    [SerializeField] Image clockRedFill;

    [SerializeField] Transform clock;
    [SerializeField] Transform clockNeedle;

    private float initBarValue = 1080f;
    private bool hurryUp = false;

    private void Start()
    {
        initBarValue = barValue.sizeDelta.x;

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

    private float GetSizeFromInitValue(float scale)
    {
        return initBarValue * scale;
    }

    public void SetBarValue(float value)
    {
        barValue.sizeDelta = new Vector3(initBarValue * value, barValue.sizeDelta.y);
        clockRedFill.fillAmount = 1 - value;
        clockNeedle.localRotation = Quaternion.Euler(new Vector3(0, 0, (1 - value) * -360));

        if (!hurryUp)
        {
            if (value < 0.25f)
            {
                hurryUp = true;
                StartCoroutine(HurryUp());
            }
        }

        if(hurryUp)
        {
            if (value >= 0.25f)
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
