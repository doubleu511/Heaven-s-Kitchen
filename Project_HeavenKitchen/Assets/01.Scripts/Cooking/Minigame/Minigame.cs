using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    protected MinigameHandler handler;

    private void Awake()
    {
        handler = FindObjectOfType<MinigameHandler>();
    }

    public abstract void StartMinigame(MinigameInfo minigameInfo);

    public virtual void EndMinigame()
    {
        handler.MinigameEnd();
    }
}
