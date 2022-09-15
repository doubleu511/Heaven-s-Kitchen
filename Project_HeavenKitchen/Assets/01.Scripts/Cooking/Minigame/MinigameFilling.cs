using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameFilling : Minigame
{
    [SerializeField] Image kettleImg;
    [SerializeField] Image liquidImg;
    [SerializeField] Image nextLiquidImg;
    [SerializeField] SpriteAnimator waterStreamAnim;
    [SerializeField] Image teabagInImg;
    [SerializeField] Image teabagOutImg;

    Sprite[] streamSprs;
    Sprite[] teaBagSprs;
    Sprite[] nextLiquidSprs;

    public float strength = 0f;
    public float remainLiquid = 100f;
    public float saveLiquid = 0f;

    float beforeMousePosY = 0f;
    int streamAnimIndex = -1; // 4프레임씩 분할해야함.

    private void Awake()
    {
        waterStreamAnim.refreshNativeSizeEveryFrame = true;
    }

    protected override void Start()
    {
        mainIngredientObjectUI.clickAction += () =>
        {
            EndMinigame(saveLiquid > 80);
        };
    }

    public override void StartMinigame(MinigameInfo Info)
    {
        print("미니게임 시작");
        base.StartMinigame(Info);
        transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        minigameInfo.FindIngredientsSprites("Streaming", out streamSprs);
        minigameInfo.FindIngredientsSprites("TeaBag", out teaBagSprs);
        minigameInfo.FindIngredientsSprites("NextLiquid", out nextLiquidSprs);
        teabagInImg.sprite = teaBagSprs[0];
        teabagOutImg.sprite = teaBagSprs[1];
        nextLiquidImg.sprite = nextLiquidSprs[0];

        CookingManager.Minigame.ShowProgress(true);
        CookingManager.Minigame.ShowStartText("부워라!"); // TO DO : 번역
    }

    private void Update()
    {
        if (!IsMinigameOpen) return; // 안열려있을땐 감지할 필요가 없다.(뿌리기도)
        if (isCleared)
        {
            mainIngredientObjectUI.SetClickAble();

            return;
        } // 클리어되면 업데이트를 돌릴 필요가 없다.

        if (Input.GetMouseButton(0))
        {
            float mousePosY = Input.mousePosition.y;
            float deltaPos = mousePosY - beforeMousePosY;

            float strengthClamp = strength + (deltaPos * 0.2f);
            strength = Mathf.Clamp(strengthClamp, 0, 100);

            kettleImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 16 + 0.39f * strength));

            beforeMousePosY = Input.mousePosition.y;
        }
        else
        {
            //upAndDownTipImg.color = new Color(upAndDownTipImg.color.r, upAndDownTipImg.color.g, upAndDownTipImg.color.b, Mathf.Sin(Time.time * 5) * 0.5f + 0.7f);
            beforeMousePosY = Input.mousePosition.y;
        }

        if (remainLiquid > 0)
        {
            float waterStreamStrength = 0f;

            if (remainLiquid > 100 - strength)
            {
                waterStreamAnim.gameObject.SetActive(true);
                waterStreamStrength = Time.deltaTime * (0.1f * (strength + remainLiquid));

                if (remainLiquid + strength >= 150)
                {
                    AnimIndexChange(3);
                }
                else
                {
                    if (remainLiquid + strength >= 135)
                    {
                        AnimIndexChange(2);
                    }
                    else if (remainLiquid + strength >= 115)
                    {
                        AnimIndexChange(1);
                    }
                    else
                    {
                        AnimIndexChange(0);
                    }

                    saveLiquid += waterStreamStrength;
                    liquidImg.rectTransform.sizeDelta = new Vector2(liquidImg.rectTransform.sizeDelta.x, 350 + 1.62f * saveLiquid);
                    nextLiquidImg.rectTransform.sizeDelta = new Vector2(liquidImg.rectTransform.sizeDelta.x, 350 + 1.62f * saveLiquid);
                    nextLiquidImg.color = new Color(1, 1, 1, saveLiquid / 100f);
                    progressValue = (int)saveLiquid;
                    CookingManager.Minigame.ShowProgress(progressValue);
                }
            }
            else
            {
                waterStreamAnim.gameObject.SetActive(false);
            }

            remainLiquid -= waterStreamStrength;
        }
        else
        {
            isCleared = true;
            minigameParent.OnFinished();
            waterStreamAnim.gameObject.SetActive(false);
            //upAndDownTipImg.gameObject.SetActive(false);
            CookingManager.Minigame.HideProgress();
        }
    }

    public override void EndMinigame(bool clear)
    {
        transform.parent.GetComponent<Image>().color = Color.white;

        if (clear)
        {
            if (CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
            {
                CookingManager.Minigame.HideProgress();
                CookingManager.Minigame.IngredientAddAnimation(minigameInfo.reward, transform.position, mainIngredientObjectUI.GetComponent<RectTransform>().sizeDelta / 2, addIndex);
                base.EndMinigame(true);
            }
        }
        else
        {
            CookingManager.Minigame.HideProgress();
            base.EndMinigame(false);
        }
    }

    private void AnimIndexChange(int index)
    {
        if(index != streamAnimIndex)
        {
            streamAnimIndex = index;

            Sprite[] set = new Sprite[4];
            Array.Copy(streamSprs, streamAnimIndex * 4, set, 0, 4);

            waterStreamAnim.SetSprites(set);
            waterStreamAnim.StartAnimation();
        }
    }
}
