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

    protected MinigameHandler handler;
    protected MinigameInfo minigameInfo;
    [SerializeField] protected Image progressIcon;

    private void Awake()
    {
        handler = FindObjectOfType<MinigameHandler>();
    }

    public virtual void StartMinigame(MinigameInfo info)
    {
        isPlaying = true;
        minigameInfo = info;
        handler.ShowProgress(true);
        minigameParent.OnMinigameStart();
    }

    public virtual void EndMinigame()
    {
        isPlaying = false;
        handler.MinigameEnd(this);
        minigameParent.OnMinigameEnd();

        // 미니게임에 성공했다면 딕셔너리에 해당 리워드 키의 값을 1 추가한다.
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

        Destroy(this.gameObject);
    }

    public virtual void OnWindowOpen()
    {
        if (IsMinigameOpen)
        {
            handler.ShowProgress(progressValue, false);
        }
    }

    public virtual void OnWindowClose()
    {
        if (IsMinigameOpen)
        {
            handler.HideProgress();
        }
    }

    public float GetProcressValue()
    {
        return progressValue / 100f;
    }

}
