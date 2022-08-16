using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameSprinkling : Minigame
{
    public Image sprinkleBaseImg;    // �Ѹ� ���ϴ� �� (�� : �ָԹ信�� ��)
    public Image sprinkledObjectImg;  // �ѷ����°� (�� : �ָԹ信�� �ĸ�ī��)
    public Image sprinkledObjectCaseImg;  // �ѷ����°��� ��

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
        print("�̴ϰ��� ����");
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
        handler.ShowStartText("�ѷ���!"); // TO DO : ����
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
        } // Ŭ����Ǹ� ������Ʈ�� ���� �ʿ䰡 ����.
        if (!IsMinigameOpen) return; // �ȿ��������� ������ �ʿ䰡 ����.(��ġ���)

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
