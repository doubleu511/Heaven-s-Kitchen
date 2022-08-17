using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameSprinkling : Minigame
{
    public Image sprinkleBaseImg;    // 뿌림 당하는 것 (예 : 주먹밥에선 밥)
    public Image sprinkledObjectImg;  // 뿌려지는것 (예 : 주먹밥에선 후리카케)
    public Image sprinkledObjectCaseImg;  // 뿌려지는것의 통

    public Image upAndDownTipImg;

    private IngredientSO sprinkledIngredient;

    private int curRepeatCount = 0;
    private int repeatCount = 0;

    private bool shakeFlag = false;

    Sprite[] savedSprs;
    Sprite[] progressSprs;

    float beforeMousePosY;

    public override void StartMinigame(MinigameInfo Info)
    {
        print("미니게임 시작");
        base.StartMinigame(Info);
        repeatCount = Info.repeatCount;

        sprinkledIngredient = minigameInfo.FindIngredientsSprites("Sprinkling", out savedSprs);
        minigameInfo.reward.FindSprites("Sprinkling_Progress", out progressSprs);

        sprinkledObjectCaseImg.sprite = sprinkledIngredient.ingredientDefaulrSpr;
        if (sprinkledIngredient.GetType() == typeof(HasTagIngredientSO))
        {
            HasTagIngredientSO tagIngredient = (HasTagIngredientSO)sprinkledIngredient;
            tagIngredient.CreateTag(sprinkledObjectCaseImg);
        }

        sprinkledObjectImg.gameObject.SetActive(false);
        sprinkledObjectImg.sprite = savedSprs[0];
        progressIcon.sprite = progressSprs[0];

        handler.ShowProgress(true);
        handler.ShowStartText("뿌려라!"); // TO DO : 번역
    }

    private void Update()
    {
        if (!IsMinigameOpen) return; // 안열려있을땐 감지할 필요가 없다.(뿌리기도)
        if (isCleared)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EndMinigame();
            }

            return;
        } // 클리어되면 업데이트를 돌릴 필요가 없다.

        if (Input.GetMouseButton(0))
        {
            float mousePosY = Input.mousePosition.y;
            float deltaPos = mousePosY - beforeMousePosY;

            float caseClampY = sprinkledObjectCaseImg.rectTransform.anchoredPosition.y + deltaPos;

            caseClampY = Mathf.Clamp(caseClampY, 150, 300);
            sprinkledObjectCaseImg.rectTransform.anchoredPosition = new Vector2(sprinkledObjectCaseImg.rectTransform.anchoredPosition.x, caseClampY);

            beforeMousePosY = Input.mousePosition.y;

            if(deltaPos > 0)
            {
                shakeFlag = true;
            }

            if(shakeFlag)
            {
                if(deltaPos < -4)
                {
                    // TO DO : 이펙트 추가
                    progressValue += 15;
                    handler.ShowProgress(progressValue);
                    shakeFlag = false;
                }
            }
        }
        else
        {
            beforeMousePosY = Input.mousePosition.y;
        }

        if (progressValue >= 100)
        {
            curRepeatCount++;
            progressIcon.sprite = progressSprs[curRepeatCount];

            if (curRepeatCount == repeatCount)
            {
                isCleared = true;
                upAndDownTipImg.gameObject.SetActive(false);
                handler.HideProgress();
            }
            else
            {
                progressValue = 0;
            }
        }
        else if (progressValue >= 80)
        {
            sprinkledObjectImg.sprite = savedSprs[2];
        }
        else if (progressValue >= 50)
        {
            sprinkledObjectImg.sprite = savedSprs[1];
        }
        else if (progressValue >= 20)
        {
            sprinkledObjectImg.gameObject.SetActive(true);
            sprinkledObjectImg.sprite = savedSprs[0];
        }
        else
        {
            sprinkledObjectImg.gameObject.SetActive(false);
        }
    }

    public override void EndMinigame()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
        {
            print("미니게임 끝");
            handler.IngredientAddAnimation(minigameInfo.reward, transform.position, sprinkleBaseImg.rectTransform.sizeDelta / 2, addIndex);
            base.EndMinigame();
        }
    }
}
