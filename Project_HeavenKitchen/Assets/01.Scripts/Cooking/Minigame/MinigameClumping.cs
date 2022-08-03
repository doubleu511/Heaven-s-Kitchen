using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameClumping : Minigame
{
    MinigameInfo minigameInfo; //test;

    public override void StartMinigame(MinigameInfo Info)
    {
        // 테스트 용도!!!!!!!!!!!!!!
        print("미니게임 시작");
        minigameInfo = Info;
        EndMinigame();
    }

    public override void EndMinigame()
    {
        print("미니게임 끝");
        CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward);
        base.EndMinigame();
    }
}
