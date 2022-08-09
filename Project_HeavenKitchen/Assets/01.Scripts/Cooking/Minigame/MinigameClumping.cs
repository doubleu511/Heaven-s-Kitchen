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
        print("�̴ϰ��� ����");
        base.StartMinigame(Info);
        savedSprs = minigameInfo.ingredients[0].ingredientMinigameSprite.Find(x => x.spritesName.Equals("Clumped")).sprites;
        clumpingImg.sprite = savedSprs[0];
        handler.ShowProgress(true);
        handler.ShowStartText("���Ķ�!");
    }

    private void Update()
    {
        if (isCleared) return; // Ŭ����Ǹ� ������Ʈ�� ���� �ʿ䰡 ����.
        if (!IsMinigameOpen) return; // �ȿ��������� ������ �ʿ䰡 ����.(��ġ���)

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
                        print("����!");
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
            print("�̴ϰ��� ��");
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
