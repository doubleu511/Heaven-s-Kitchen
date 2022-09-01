using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtensilsUIHandler : MonoBehaviour
{
    [SerializeField] UtensilsUI UIPrefab;
    [SerializeField] UtensilsCircleInventoryUI circlePrefab;

    [SerializeField] Transform circlePrefabBoxTrm;

    [SerializeField] Sprite plusSpr;
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
}
