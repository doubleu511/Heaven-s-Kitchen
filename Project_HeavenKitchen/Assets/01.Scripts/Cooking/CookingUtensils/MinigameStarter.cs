using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameStarter : InteractiveObject
{
    public CookingUtensilsSO cookingUtensilsSO;
    public List<UtensilsInventory> utensilsInventories = new List<UtensilsInventory>();
    private List<MinigameInfo> minigameInfos = new List<MinigameInfo>();

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
}

[System.Serializable]
public class UtensilsInventory
{
    public int minigameId;
    public IngredientSO[] ingredients;
}