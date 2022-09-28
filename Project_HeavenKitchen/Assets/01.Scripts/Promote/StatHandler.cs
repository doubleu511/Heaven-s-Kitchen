using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;

public class StatHandler : MonoBehaviour
{
    #region Dictionaries
    public static Dictionary<StatType, int> statDic = new Dictionary<StatType, int>()
    {
        {StatType.KNOWLEDGE, 100 },
        {StatType.DILIGENCE, 100 },
        {StatType.SENSE, 100 },
        {StatType.SPEED, 100 },
        {StatType.HEALTH, 100 },
        {StatType.STRESS, 78 },
    };

    public static Dictionary<RankType, Color32> rankColorDic = new Dictionary<RankType, Color32>()
    {
        { RankType.RANK_D, new Color32(180, 157, 209, 255) },
        { RankType.RANK_C, new Color32(161, 190, 229, 255) },
        { RankType.RANK_B, new Color32(169, 209, 197, 255) },
        { RankType.RANK_A, new Color32(224, 166, 180, 255) },
        { RankType.RANK_S, new Color32(243, 204, 146, 255) }
    };

    public static Dictionary<RankType, string> rankStrDic = new Dictionary<RankType, string>()
    {
        { RankType.RANK_D, "D" },
        { RankType.RANK_C, "C" },
        { RankType.RANK_B, "B" },
        { RankType.RANK_A, "A" },
        { RankType.RANK_S, "S" }
    };
    #endregion

    public static int stressMax = 100;
    public static event Action<StatType, bool> onStatChanged;

    public static bool TryResultFromStress()
    {
        float percent = statDic[StatType.STRESS] / (float)stressMax;

        if(percent <= 0.5f)
        {
            return true;
        }
        else if (percent <= 0.75f)
        {
            return UtilClass.ProbabilityCalculate(75);
        }
        else
        {
            return UtilClass.ProbabilityCalculate(50);
        }
    }
}
