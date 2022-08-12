using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public MinigameStarter minigameParent { get; set; }
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

    protected GameObject repeatCountUI;

    private void Awake()
    {
        handler = FindObjectOfType<MinigameHandler>();
        repeatCountUI = Global.Resource.Load<GameObject>("Minigames/UI/Minigame_RepeatCountUI");
    }

    public virtual void StartMinigame(MinigameInfo info)
    {
        isPlaying = true;
        minigameInfo = info;
        handler.ShowProgress(true);
    }

    public virtual void EndMinigame()
    {
        isPlaying = false;
        handler.MinigameEnd(this);
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
}
