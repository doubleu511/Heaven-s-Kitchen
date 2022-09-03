using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtensilsUIHandler : MonoBehaviour
{
    [SerializeField] UtensilsUI UIPrefab;
    [SerializeField] UtensilsCircleInventoryUI circlePrefab;

    [SerializeField] Transform circlePrefabBoxTrm;

    [Header("Sprites")]
    [SerializeField] Sprite plusSpr;
    [SerializeField] Sprite checkSpr;
    [SerializeField] Sprite warningSpr;
    [SerializeField] Sprite deadSpr;

    [Space(10)]
    [SerializeField] Sprite normalBalloonSpr;
    [SerializeField] Sprite spikeBalloonSpr;
    public static Sprite PlusSpr;

    Dictionary<MinigameStarter, UtensilsUI> utensilsPairsDic = new Dictionary<MinigameStarter, UtensilsUI>();

    private void Awake()
    {
        PlusSpr = plusSpr;
        Global.Pool.CreatePool<UtensilsUI>(UIPrefab.gameObject, transform, 8);
        Global.Pool.CreatePool<UtensilsCircleInventoryUI>(circlePrefab.gameObject, circlePrefabBoxTrm, 10);
    }

    public void InitUI(MinigameStarter utensils, Vector2 pos)
    {
        UtensilsUI utensilsUI = Global.Pool.GetItem<UtensilsUI>();
        utensilsUI.transform.position = pos;
        utensilsPairsDic[utensils] = utensilsUI;

        utensilsUI.InventoryRefresh(utensils.utensilsInventories);
        HideProcessBar(utensils);
    }

    public void RefreshUtensilsInventory()
    {
        UtensilsUI utensilsUI = utensilsPairsDic[CookingManager.Global.CurrentUtensils];
        utensilsUI.InventoryRefresh(CookingManager.Global.CurrentUtensils.utensilsInventories);
    }

    public void HideProcessBar(MinigameStarter utensils)
    {
        UtensilsUI utensilsUI = utensilsPairsDic[utensils];
        utensilsUI.HideProcessBar();
    }

    public void SetProcessBar(MinigameStarter utensils, float value)
    {
        UtensilsUI utensilsUI = utensilsPairsDic[utensils];
        utensilsUI.SetProcessBar(value);
    }

    public void SetBackgroundImage(MinigameStarter utensils, UtensilsCircleInventoryUI.BackgroundSpriteType spriteType)
    {
        UtensilsUI utensilsUI = utensilsPairsDic[utensils];

        switch (spriteType)
        {
            case UtensilsCircleInventoryUI.BackgroundSpriteType.NORMAL:
                utensilsUI.SetBackgroundImage(normalBalloonSpr);
                break;
            case UtensilsCircleInventoryUI.BackgroundSpriteType.SPIKE:
                utensilsUI.SetBackgroundImage(spikeBalloonSpr);
                break;
        }
    }

    public void SetStatusImage(MinigameStarter utensils, UtensilsCircleInventoryUI.StatusSpriteType spriteType)
    {
        UtensilsUI utensilsUI = utensilsPairsDic[utensils];

        switch(spriteType)
        {
            case UtensilsCircleInventoryUI.StatusSpriteType.NONE:
                utensilsUI.SetStatusImage(null);
                break;
            case UtensilsCircleInventoryUI.StatusSpriteType.CHECK:
                utensilsUI.SetStatusImage(checkSpr);
                break;
            case UtensilsCircleInventoryUI.StatusSpriteType.WARNING:
                utensilsUI.SetStatusImage(warningSpr);
                break;
            case UtensilsCircleInventoryUI.StatusSpriteType.DEAD:
                utensilsUI.SetStatusImage(deadSpr);
                break;
        }
    }

    public void SetStatusAnimation(MinigameStarter utensils, UtensilsCircleInventoryUI.StatusAnimationType animationType)
    {
        UtensilsUI utensilsUI = utensilsPairsDic[utensils];
        utensilsUI.SetStatusAnimation(animationType);
    }
}
