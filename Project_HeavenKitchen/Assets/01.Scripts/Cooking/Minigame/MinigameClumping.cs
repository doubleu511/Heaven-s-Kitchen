using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinigameClumping : Minigame
{
    public Image clumpingImg;
    public Transform[] clumpingPoses;
    public List<int> posCloserIndex = new List<int>();

    private int currentIndex = -1;
    private int targetIndex;
    private int nextTargetIndex;

    Sprite[] savedSprs;

    public override void StartMinigame(MinigameInfo Info)
    {
        print("미니게임 시작");
        base.StartMinigame(Info);
        savedSprs = minigameInfo.ingredients[0].ingredientMinigameSprite.Find(x => x.spritesName.Equals("Clumped")).sprites;
        clumpingImg.sprite = savedSprs[0];
        handler.ShowProgress(true);
        handler.ShowStartText("뭉쳐라!");
    }

    private void Update()
    {
        if (isCleared) return; // 클리어되면 업데이트를 돌릴 필요가 없다.
        if (!IsMinigameOpen) return; // 안열려있을땐 감지할 필요가 없다.(뭉치기는)

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            posCloserIndex.Sort((x, y) => (mousePos - clumpingPoses[x].transform.position).sqrMagnitude.CompareTo((mousePos - clumpingPoses[y].transform.position).sqrMagnitude));

            if(currentIndex == -1)
            {
                currentIndex = posCloserIndex[0];
                targetIndex = (currentIndex + 1) % posCloserIndex.Count;
                nextTargetIndex = (currentIndex + 2) % posCloserIndex.Count;
            }
            else
            {
                currentIndex = posCloserIndex[0];

                if (currentIndex == targetIndex)
                {
                    if(posCloserIndex[1] == nextTargetIndex)
                    {
                        //Check!
                        print("성공!");
                        targetIndex = (currentIndex + 1) % posCloserIndex.Count;
                        nextTargetIndex = (currentIndex + 2) % posCloserIndex.Count;

                        progressValue += 5;
                        handler.ShowProgress(progressValue);
                    }
                }
            }
        }

        if(progressValue >= 100)
        {
            isCleared = true;
            handler.HideProgress();
        }
        else if(progressValue >= 66)
        {
            clumpingImg.sprite = savedSprs[2];
        }
        else if(progressValue >= 33)
        {
            clumpingImg.sprite = savedSprs[1];
        }
    }

    public override void EndMinigame()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward))
        {
            print("미니게임 끝");
            base.EndMinigame();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isCleared)
        {
            EndMinigame();
        }
    }
}
