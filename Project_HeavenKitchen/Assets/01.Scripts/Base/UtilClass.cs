using System.Collections;
using System.Collections.Generic;
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