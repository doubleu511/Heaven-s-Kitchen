using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UtilClass
{
    public static bool IsIncludeFlag<T>(T from, T to)
    {
        int _from = (int)(object)from;
        int _to = (int)(object)to;

        if((_from & _to) != 0)
        {
            return true;
        }

        return false;
    }

    public static float GetAngleFromVector(Vector3 dir)
    {
        float radians = Mathf.Atan2(dir.y, dir.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public static IEnumerator TextAnimationCoroutine(TextMeshProUGUI text, int current, int target)
    {
        float fCurrent = current;
        float fTarget = target;

        text.text = $"{current}";

        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        if (current < target)
        {
            while (fCurrent < fTarget)
            {
                fCurrent += offset * Time.deltaTime;
                text.text = ((int)fCurrent).ToString();
                yield return null;
            }
        }
        else if (current > target)
        {
            while (fCurrent > fTarget)
            {
                fCurrent += offset * Time.deltaTime;
                text.text = ((int)fCurrent).ToString();
                yield return null;
            }
        }

        text.text = $"{target}";
    }

    public static bool ProbabilityCalculate(float percent)
    {
        float percentValue = percent / 100;
        float randomvalue = Random.Range(0f, 1f);

        return randomvalue <= percentValue;
    }

    public static void ForceRefreshSize(Transform transform)
    {
        // ContentSizeFitter를 강제 새로고침한다.
        ContentSizeFitter[] csfs = transform.GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < csfs.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csfs[i].transform);
        }
    }

    public static void ForceRefreshGrid(Transform transform)
    {
        // ContentSizeFitter를 강제 새로고침한다.
        GridLayoutGroup[] glgs = transform.GetComponentsInChildren<GridLayoutGroup>();
        for (int i = 0; i < glgs.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)glgs[i].transform);
        }
    }
}