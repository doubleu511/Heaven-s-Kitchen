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

    public Transform[] sprinklingPoses;

    public Image upAndDownTipImg;

    private IngredientSO sprinkledIngredient;

    private int curRepeatCount = 0;
    private int repeatCount = 0;

    private int currentIndex = -1;
    private int targetIndex;
    private int nextTargetIndex;

    Sprite[] savedSprs;
    Sprite[] progressSprs;

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
            HasTagIngredientSO test = (HasTagIngredientSO)sprinkledIngredient;
            test.CreateTag(sprinkledObjectCaseImg);
        }

        sprinkledObjectImg.sprite = savedSprs[0];
        progressIcon.sprite = progressSprs[0];

        handler.ShowProgress(true);
        handler.ShowStartText("뿌려라!"); // TO DO : 번역
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

        }
    }

    public override void EndMinigame()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
        {

        }
    }
}
