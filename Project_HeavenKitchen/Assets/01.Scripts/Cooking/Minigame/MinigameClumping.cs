using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameClumping : Minigame
{
    MinigameInfo minigameInfo; //test;

    public override void StartMinigame(MinigameInfo Info)
    {
        // �׽�Ʈ �뵵!!!!!!!!!!!!!!
        print("�̴ϰ��� ����");
        minigameInfo = Info;
        EndMinigame();
    }

    public override void EndMinigame()
    {
        print("�̴ϰ��� ��");
        CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward);
        base.EndMinigame();
    }
}
