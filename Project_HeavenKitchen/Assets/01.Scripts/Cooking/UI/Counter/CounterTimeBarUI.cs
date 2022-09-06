using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterTimeBarUI : MonoBehaviour
{
    [SerializeField] RectTransform barValue;
    [SerializeField] Image clockIconImg;
    [SerializeField] Image clockRedFill;

    [SerializeField] Transform clock;
    [SerializeField] Transform clockNeedle;

    private bool hurryUp = false;

    private void Start()
    {
        clockIconImg.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        clockIconImg.transform.DOLocalRotate(new Vector3(0, 0, 15f), 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void SetBarValue(float value)
    {
        barValue.transform.localScale = new Vector3(value, 1);
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
