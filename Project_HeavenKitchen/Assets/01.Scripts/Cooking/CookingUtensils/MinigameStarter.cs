using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameStarter : InteractiveObject
{
    public Define.MinigameType[] canPlayMinigameTypes;
    public UtensilsInventory[] utensilsInventories;

    public override void OnInteract()
    {
        RecipeSO[] recipes = CookingManager.GetRecipes();
        List<MinigameInfo> minigameInfos = new List<MinigameInfo>();

        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].recipe.Length; j++)
            {
                for (int k = 0; k < canPlayMinigameTypes.Length; k++)
                {
                    if (recipes[i].recipe[j].minigameType == canPlayMinigameTypes[k])
                    {
                        minigameInfos.Add(recipes[i].recipe[j]);
                    }
                }
            }
        }

        MinigameHandler handler = FindObjectOfType<MinigameHandler>();
        handler.ReceiveInfo(minigameInfos.ToArray());
    }
}

[System.Serializable]
public class UtensilsInventory
{
    public int minigameIndex;
    public IngredientSO[] ingredients;
}