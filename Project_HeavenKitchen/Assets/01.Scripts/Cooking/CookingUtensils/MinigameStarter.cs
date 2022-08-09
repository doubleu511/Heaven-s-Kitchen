using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameStarter : InteractiveObject
{
    public CookingUtensilsSO cookingUtensilsSO;
    public List<UtensilsInventory> utensilsInventories = new List<UtensilsInventory>();

    public override void OnInteract()
    {
        if (CookingManager.Global.CurrentUtensils != null) return;

        RecipeSO[] recipes = CookingManager.GetRecipes();
        List<MinigameInfo> minigameInfos = new List<MinigameInfo>();

        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].recipe.Length; j++)
            {
                for (int k = 0; k < cookingUtensilsSO.canPlayMinigameTypes.Length; k++)
                {
                    if (recipes[i].recipe[j].minigameType == cookingUtensilsSO.canPlayMinigameTypes[k])
                    {
                        minigameInfos.Add(recipes[i].recipe[j]);
                    }
                }
            }
        }

        InitInventories(minigameInfos.ToArray());

        CookingManager.Global.CurrentUtensils = this;
        MinigameHandler handler = FindObjectOfType<MinigameHandler>();
        handler.ReceiveInfo(cookingUtensilsSO, minigameInfos.ToArray());
    }

    public void InitInventories(MinigameInfo[] minigameInfos)
    {
        if (utensilsInventories.Count == 0)
        {
            for (int i = 0; i < minigameInfos.Length; i++)
            {
                UtensilsInventory inventory = new UtensilsInventory();

                inventory.minigameIndex = i;
                inventory.ingredients = new IngredientSO[minigameInfos[i].ingredients.Length];

                utensilsInventories.Add(inventory);
            }
        }
    }
}

[System.Serializable]
public class UtensilsInventory
{
    public int minigameIndex;
    public IngredientSO[] ingredients;
}