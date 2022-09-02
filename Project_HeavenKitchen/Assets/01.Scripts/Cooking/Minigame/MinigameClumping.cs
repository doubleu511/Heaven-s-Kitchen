using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameClumping : Minigame
{
    public Image clumpingImg;
    public Transform[] clumpingPoses;
    public List<int> posCloserIndex = new List<int>();

    public Image rotateTipImg;

    private int curRepeatCount = 0;
    private int repeatCount = 0;

    private int currentIndex = -1;
    private int targetIndex;
    private int nextTargetIndex;

    Sprite[] savedSprs;
    Sprite[] progressSprs;

    public override void StartMinigame(MinigameInfo Info)
    {
        print("�̴ϰ��� ����");
        base.StartMinigame(Info);

        minigameInfo.ingredients[0].FindSprites("Clumped", out savedSprs);
        clumpingImg.sprite = savedSprs[0];

        repeatCount = Info.repeatCount;

        minigameInfo.reward.FindSprites("Clumped_Progress", out progressSprs);
        progressIcon.sprite = progressSprs[0];

        rotateTipImg.gameObject.SetActive(true);
        handler.ShowProgress(true);
        handler.ShowStartText("���Ķ�!"); // TO DO : ����
    }

    private void Update()
    {
        if (!IsMinigameOpen) return; // �ȿ��������� ������ �ʿ䰡 ����.(��ġ���)
        if (isCleared)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EndMinigame();
            }

            return;
        } // Ŭ����Ǹ� ������Ʈ�� ���� �ʿ䰡 ����.

        if (Input.GetMouseButton(0))
        {
            rotateTipImg.color = new Color(rotateTipImg.color.r, rotateTipImg.color.g, rotateTipImg.color.b, 0.2f);

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

                        progressValue += 2;
                        handler.ShowProgress(progressValue);
                    }
                }
            }
        }
        else
        {
            rotateTipImg.color = new Color(rotateTipImg.color.r, rotateTipImg.color.g, rotateTipImg.color.b, Mathf.Sin(Time.time * 5) * 0.5f + 0.7f);
        }

        rotateTipImg.transform.Rotate(Vector3.forward * -100 * Time.deltaTime);

        if (progressValue >= 100)
        {
            curRepeatCount++;
            progressIcon.sprite = progressSprs[curRepeatCount];

            if (curRepeatCount == repeatCount)
            {
                isCleared = true;
                minigameParent.OnFinished();
                rotateTipImg.gameObject.SetActive(false);
                handler.HideProgress();
            }
            else
            {
                clumpingImg.sprite = savedSprs[0];
                progressValue = 0;
                handler.ShowProgress(progressValue);
            }
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
        if(CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
        {
            print("�̴ϰ��� ��");
            handler.IngredientAddAnimation(minigameInfo.reward, transform.position, clumpingImg.rectTransform.sizeDelta / 2, addIndex);
            base.EndMinigame();
        }
    }
}
