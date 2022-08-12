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
    public Transform repeatCountTrm;

    private int curRepeatCount = 0;
    private int repeatCount = 0;
    private List<Image> repeatCountUIs = new List<Image>();

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

        repeatCount = Info.repeatCount;
        for(int i =0; i < repeatCount;i++)
        {
            Image ui = Instantiate(repeatCountUI, repeatCountTrm).GetComponent<Image>();
            ui.sprite = savedSprs[savedSprs.Length - 1];
            ui.color = new Color(103 / 255f, 103 / 255f, 103 / 255f, 177 / 255f);
            repeatCountUIs.Add(ui);
        }

        rotateTipImg.gameObject.SetActive(true);
        handler.ShowProgress(true);
        handler.ShowStartText("뭉쳐라!");
    }

    private void Update()
    {
        if (isCleared)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EndMinigame();
            }

            return;
        } // 클리어되면 업데이트를 돌릴 필요가 없다.
        if (!IsMinigameOpen) return; // 안열려있을땐 감지할 필요가 없다.(뭉치기는)

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
                        print("성공!");
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
            repeatCountUIs[curRepeatCount - 1].color = Color.white;

            if (curRepeatCount == repeatCount)
            {
                isCleared = true;
                rotateTipImg.gameObject.SetActive(false);
                handler.HideProgress();
            }
            else
            {
                clumpingImg.sprite = savedSprs[0];
                progressValue = 0;
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
            print("미니게임 끝");
            handler.IngredientAddAnimation(minigameInfo.reward, transform.position, clumpingImg.rectTransform.sizeDelta / 2, addIndex);
            base.EndMinigame();
        }
    }
}
