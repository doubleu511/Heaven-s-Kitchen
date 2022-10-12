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
        {StatType.STRESS, 0 },
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

        if (percent <= 0.5f)
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

    // 현재 육성화면에서 알아서 스트레스 더해주고 빼준다.
    public static void TryAddStat(bool isSuccess)
    {
        PromoteType promoteType = PromoteManager.PromoteScheduleList[PromoteManager.Promote.ScheduleIndex - 1];
        StatType stat = promoteType.studySO.studyType;
        int level = promoteType.level;

        if (stat != StatType.STRESS && isSuccess)
        {
            statDic[stat] += promoteType.studySO.studys[level].gainStatValue;
            statDic[stat] = Mathf.Clamp(statDic[stat], 0, 999);
        }

        if (promoteType.studySO.studys[level].stressValue != 0)
        {
            statDic[StatType.STRESS] += promoteType.studySO.studys[level].stressValue;
            statDic[StatType.STRESS] = Mathf.Clamp(statDic[StatType.STRESS], 0, stressMax);
            onStatChanged(StatType.STRESS, true);
        }

        onStatChanged(stat, true);
    }

    // 그냥 따로 스탯 추가해주는 함수
    public static void AddStat(StatType statType, int value)
    {
        statDic[statType] += value;
        if (statType != StatType.STRESS)
        {
            statDic[statType] = Mathf.Clamp(statDic[statType], 0, 999);
        }
        else
        {
            statDic[statType] = Mathf.Clamp(statDic[statType], 0, stressMax);
        }

        onStatChanged(statType, true);
    }

    public static void StatReset()
    {
        statDic = new Dictionary<StatType, int>()
    {
        {StatType.KNOWLEDGE, 100 },
        {StatType.DILIGENCE, 100 },
        {StatType.SENSE, 100 },
        {StatType.SPEED, 100 },
        {StatType.HEALTH, 100 },
        {StatType.STRESS, 0 },
    };
    }
}
