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
        {StatType.KNOWLEDGE, 0 },
        {StatType.DILIGENCE, 0 },
        {StatType.SENSE, 0 },
        {StatType.SPEED, 0 },
        {StatType.HEALTH, 0 },
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

    public static event Action onStatChanged;
}
