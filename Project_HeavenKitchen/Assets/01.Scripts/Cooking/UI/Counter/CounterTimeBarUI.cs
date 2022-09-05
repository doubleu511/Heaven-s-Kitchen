using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterTimeBarUI : MonoBehaviour
{
    [SerializeField] RectTransform barValue;
    [SerializeField] Image clockIconImg;

    public void SetBarValue(float value)
    {
        barValue.transform.localScale = new Vector3(value, 1);
    }
}
