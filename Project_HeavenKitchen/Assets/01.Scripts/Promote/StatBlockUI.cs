using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] Define.StatType statType;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI valueText;

    private int savedValue = 0;

    private void Start()
    {
        StatHandler.onStatChanged += OnStatChanged;
        OnStatChanged(statType, false);
    }

    private void OnStatChanged(Define.StatType stat, bool animation)
    {
        if(stat == statType)
        {
            if(animation)
            {
                StartCoroutine(UtilClass.TextAnimationCoroutine(valueText, savedValue, StatHandler.statDic[statType]));
            }
            else
            {
                valueText.text = StatHandler.statDic[statType].ToString();
            }

            savedValue = StatHandler.statDic[statType];

            Define.RankType currentRank = GetRankScore(savedValue);
            rankText.text = StatHandler.rankStrDic[currentRank];
            rankText.color = StatHandler.rankColorDic[currentRank];
        }
    }

    private static Define.RankType GetRankScore(int score)
    {
        if(score >= 800)
        {
            return Define.RankType.RANK_S;
        }
        else if(score >= 600)
        {
            return Define.RankType.RANK_A;
        }
        else if(score >= 400)
        {
            return Define.RankType.RANK_B;
        }
        else if (score >= 200)
        {
            return Define.RankType.RANK_C;
        }
        else
        {
            return Define.RankType.RANK_D;
        }
    }
}
