using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameClumping : Minigame
{
    private Image clumpingImg; // 뭉쳐질 오브젝트

    public Transform[] clumpingPoses;
    public List<int> posCloserIndex = new List<int>();

    public Image rotateTipImg;

    private int curRepeatCount = 0;
    private int repeatCount = 0;

    private int currentIndex = -1;
    private int targetIndex;
    private int nextTargetIndex;
    [SerializeField] protected Image progressIcon;

    Sprite[] savedSprs;
    Sprite[] progressSprs;

    private void Awake()
    {
        clumpingImg = mainIngredientObjectUI.GetComponent<Image>();
    }

    public override void StartMinigame(MinigameInfo Info)
    {
        print("미니게임 시작");
        base.StartMinigame(Info);

        minigameInfo.ingredients[0].FindSprites("Clumped", out savedSprs);
        clumpingImg.sprite = savedSprs[0];

        repeatCount = Info.repeatCount;

        minigameInfo.reward.FindSprites("Clumped_Progress", out progressSprs);
        progressIcon.sprite = progressSprs[0];

        rotateTipImg.gameObject.SetActive(true);
        CookingManager.Minigame.ShowProgress(true);
        CookingManager.Minigame.ShowStartText("뭉쳐라!"); // TO DO : 번역
    }

    private void Update()
    {
        if (!IsMinigameOpen) return; // 안열려있을땐 감지할 필요가 없다.(뭉치기는)
        if (isCleared)
        {
            mainIngredientObjectUI.SetClickAble();

            return;
        } // 클리어되면 업데이트를 돌릴 필요가 없다.

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
                        CookingManager.Minigame.ShowProgress(progressValue);
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
                CookingManager.Minigame.HideProgress();
            }
            else
            {
                clumpingImg.sprite = savedSprs[0];
                progressValue = 0;
                CookingManager.Minigame.ShowProgress(progressValue);
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

    public override void EndMinigame(bool clear)
    {
        if(CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
        {
            print("미니게임 끝");
            CookingManager.Minigame.IngredientAddAnimation(minigameInfo.reward, transform.position, clumpingImg.rectTransform.sizeDelta / 2, addIndex);
            base.EndMinigame(clear);
        }
    }
}
