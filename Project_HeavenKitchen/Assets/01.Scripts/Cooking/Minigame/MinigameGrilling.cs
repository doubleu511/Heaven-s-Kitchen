using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameGrilling : Minigame
{
    Image grilledObjectImg; // 구워지는것

    [SerializeField] ParticleSystem grillSmokeParticle;

    Sprite[] savedSprs;
    float grillTime = 0f;
    float burnTime = 0f;

    bool isBurn = false;
    bool isWarningStrong = false;
    bool isWarning = false;

    private void Awake()
    {
        grilledObjectImg = mainIngredientObjectUI.GetComponent<Image>();
    }

    protected override void Start()
    {
        mainIngredientObjectUI.clickAction += () =>
        {
            EndMinigame(!isBurn);
        };
    }

    public override void StartMinigame(MinigameInfo Info)
    {
        print("미니게임 시작");
        base.StartMinigame(Info);

        minigameInfo.ingredients[0].FindSprites("Grilled", out savedSprs);
        grilledObjectImg.sprite = savedSprs[0];

        var main = grillSmokeParticle.main;
        main.startColor = Color.white;
        grillSmokeParticle.Play();

        CookingManager.Minigame.ShowProgress(true);
        CookingManager.Minigame.ShowStartText("구워라!"); // TO DO : 번역
    }

    private void Update()
    {
        if (isCleared)
        {
            mainIngredientObjectUI.SetClickAble();

            // 여기에 초를 세고 태운다.
            if (!isBurn)
            {
                burnTime += Time.deltaTime;
                if (burnTime > 7)
                {
                    // 탔어.
                    isBurn = true;
                    grilledObjectImg.sprite = savedSprs[3];

                    var main = grillSmokeParticle.main;
                    main.startColor = Color.gray;

                    var emission = grillSmokeParticle.emission;
                    emission.rateOverTime = 5;

                    FryingPan frypan = minigameParent.GetComponent<FryingPan>();
                    if(frypan)
                    {
                        frypan.SetSmokeState(2);
                    }

                    CookingManager.UtensilsUI.SetBackgroundImage(minigameParent, UtensilsCircleInventoryUI.BackgroundSpriteType.NORMAL);
                    CookingManager.UtensilsUI.SetStatusImage(minigameParent, UtensilsCircleInventoryUI.StatusSpriteType.DEAD);
                    CookingManager.UtensilsUI.SetStatusAnimation(minigameParent, UtensilsCircleInventoryUI.StatusAnimationType.NONE);
                }
                else if (burnTime > 5)
                {
                    if(!isWarningStrong)
                    {
                        isWarningStrong = true;
                        CookingManager.UtensilsUI.SetStatusAnimation(minigameParent, UtensilsCircleInventoryUI.StatusAnimationType.BLINK_FAST);
                    }
                }
                else if (burnTime > 3)
                {
                    if(!isWarning)
                    {
                        isWarning = true;
                        CookingManager.UtensilsUI.SetBackgroundImage(minigameParent, UtensilsCircleInventoryUI.BackgroundSpriteType.SPIKE);
                        CookingManager.UtensilsUI.SetStatusImage(minigameParent, UtensilsCircleInventoryUI.StatusSpriteType.WARNING);
                        CookingManager.UtensilsUI.SetStatusAnimation(minigameParent, UtensilsCircleInventoryUI.StatusAnimationType.BLINK);
                    }
                }
            }

            return;
        }

        grillTime += Time.deltaTime * 5;
        progressValue = (int)grillTime;
        if (IsMinigameOpen)
        {
            CookingManager.Minigame.ShowProgress(progressValue);
        }

        if (progressValue >= 100)
        {
            grilledObjectImg.sprite = savedSprs[2];
            CookingManager.UtensilsUI.SetStatusImage(minigameParent, UtensilsCircleInventoryUI.StatusSpriteType.CHECK);
            CookingManager.UtensilsUI.SetStatusAnimation(minigameParent, UtensilsCircleInventoryUI.StatusAnimationType.FADE);
            minigameParent.OnFinished();
            isCleared = true;
        }
        else if (progressValue >= 50)
        {
            grilledObjectImg.sprite = savedSprs[1];
        }
        else
        {
            grilledObjectImg.sprite = savedSprs[0];
        }
    }

    public override void EndMinigame(bool clear)
    {
        if (clear)
        {
            if (CookingManager.Player.Inventory.InventoryAdd(minigameInfo.reward, out int addIndex))
            {
                grillSmokeParticle.Stop();
                CookingManager.Minigame.HideProgress();
                CookingManager.Minigame.IngredientAddAnimation(minigameInfo.reward, transform.position, grilledObjectImg.rectTransform.sizeDelta / 2, addIndex);
                base.EndMinigame(true);
            }
        }
        else
        {
            grillSmokeParticle.Stop();
            CookingManager.Minigame.HideProgress();
            base.EndMinigame(false);
        }
    }
}
