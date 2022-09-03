using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Minigame : MonoBehaviour
{
    public MinigameStarter minigameParent { get; set; }
    public int curMinigameIndex { get; set; }
    protected bool IsMinigameOpen
    {
        get
        {
            return minigameParent == CookingManager.Global.CurrentUtensils;
        }
    }
    public bool isPlaying { get; set; } = false;
    protected bool isCleared = false;

    protected int progressValue = 0;

    protected MinigameInfo minigameInfo;

    [SerializeField] protected MinigameIngredientUI mainIngredientObjectUI;

    protected virtual void Start()
    {
        mainIngredientObjectUI.clickAction += () =>
        {
            EndMinigame(true);
        };
    }

    public virtual void StartMinigame(MinigameInfo info)
    {
        isPlaying = true;
        minigameInfo = info;
        CookingManager.Minigame.ShowProgress(true);
        minigameParent.OnMinigameStart();
    }

    public virtual void EndMinigame(bool clear)
    {
        isPlaying = false;
        CookingManager.Minigame.MinigameEnd(this);
        minigameParent.OnMinigameEnd();

        // 미니게임에 성공했다면 딕셔너리에 해당 리워드 키의 값을 1 추가한다.
        if (clear)
        {
            if (CookingManager.Global.MemoSuccessCountDic.ContainsKey(minigameInfo.reward))
            {
                CookingManager.Global.MemoSuccessCountDic[minigameInfo.reward]++;
            }
            else
            {
                CookingManager.Global.MemoSuccessCountDic[minigameInfo.reward] = 1;
            }

            MemoHandler memoHandler = FindObjectOfType<MemoHandler>();
            memoHandler.RefreshMinigameRecipes();
        }

        Destroy(this.gameObject);
    }

    public virtual void OnWindowOpen()
    {
        if (IsMinigameOpen)
        {
            CookingManager.Minigame.ShowProgress(progressValue, false);
        }
    }

    public virtual void OnWindowClose()
    {
        if (IsMinigameOpen)
        {
            CookingManager.Minigame.HideProgress();
        }
    }

    public float GetProcressValue()
    {
        return progressValue / 100f;
    }

}
