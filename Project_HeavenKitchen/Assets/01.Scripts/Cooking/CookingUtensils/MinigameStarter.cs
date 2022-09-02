using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameStarter : InteractiveObject
{
    [SerializeField] Transform inventoryAppearTrm;
    private UtensilsUIHandler utensilsInventoryHandler;

    public CookingUtensilsSO cookingUtensilsSO;
    public List<UtensilsInventory> utensilsInventories = new List<UtensilsInventory>();
    private List<MinigameInfo> minigameInfos = new List<MinigameInfo>();

    protected virtual void Awake()
    {
        utensilsInventoryHandler = FindObjectOfType<UtensilsUIHandler>();
    }

    protected virtual void Start()
    {
        InitUtensilsGUI();
    }

    public override void OnInteract()
    {
        if (CookingManager.Global.CurrentUtensils != null) return;

        RefreshInventory();
        CookingManager.Global.CurrentUtensils = this;
        MinigameHandler handler = FindObjectOfType<MinigameHandler>();
        handler.ReceiveInfo(cookingUtensilsSO, minigameInfos.ToArray());
    }

    public void RefreshInventory()
    {
        minigameInfos.Clear();

        List<MinigameInfo> allMinigameInfos = CookingManager.GetMinigames();

        for (int i = 0; i < allMinigameInfos.Count; i++)
        {
            for (int j = 0; j < cookingUtensilsSO.canPlayMinigameTypes.Length; j++)
            {
                if (allMinigameInfos[i].minigameType == cookingUtensilsSO.canPlayMinigameTypes[j])
                {
                    minigameInfos.Add(allMinigameInfos[i]);
                }
            }
        }

        InitInventories(minigameInfos.ToArray());
    }

    private void InitInventories(MinigameInfo[] minigameInfos)
    {
        if (utensilsInventories.Count == 0)
        {
            for (int i = 0; i < minigameInfos.Length; i++)
            {
                UtensilsInventory inventory = new UtensilsInventory();

                inventory.minigameId = minigameInfos[i].minigameNameTranslationId;
                inventory.ingredients = new IngredientSO[minigameInfos[i].ingredients.Length];

                utensilsInventories.Add(inventory);
            }
        }
    }

    /// <summary>
    /// 미니게임이 시작됐을 때 한번 호출됩니다.
    /// </summary>
    public virtual void OnMinigameStart() 
    {

    }

    /// <summary>
    /// 모든 미니게임이 끝났지만 리워드를 수령받지 않은 상태일 때 한번 호출됩니다.
    /// </summary>
    public virtual void OnFinished()
    {

    }

    /// <summary>
    /// 리워드를 수령했을 때 한번 호출됩니다.
    /// </summary>
    public virtual void OnMinigameEnd()
    {

    }

    public void InitUtensilsGUI()
    {
        utensilsInventoryHandler.InitUI(this, inventoryAppearTrm.position);
    }

    public void RefreshInventoryGUI()
    {
        utensilsInventoryHandler.RefreshUtensilsInventory();
    }

    public void HideProcessBar()
    {
        utensilsInventoryHandler.HideProcessBar(this);
    }

    public void SetProcessBar(float value)
    {
        utensilsInventoryHandler.SetProcessBar(this, value);
    }
}

[System.Serializable]
public class UtensilsInventory
{
    public int minigameId;
    public IngredientSO[] ingredients;
}