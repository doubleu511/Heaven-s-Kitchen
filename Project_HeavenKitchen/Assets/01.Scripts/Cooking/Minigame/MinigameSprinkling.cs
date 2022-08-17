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
        print("�̴ϰ��� ����");
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
        handler.ShowStartText("�ѷ���!"); // TO DO : ����
    }

    private void Update()
    {
        if (!IsMinigameOpen) return; // �ȿ��������� ������ �ʿ䰡 ����.(�Ѹ��⵵)
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
                    // TO DO : ����Ʈ �߰�
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
            print("�̴ϰ��� ��");
            handler.IngredientAddAnimation(minigameInfo.reward, transform.position, sprinkleBaseImg.rectTransform.sizeDelta / 2, addIndex);
            base.EndMinigame();
        }
    }
}
