using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MinigameStarter
{
    public DeleteDragableUI trashCanUI;
    public GameObject trashPrefab;

    private void Start()
    {
        trashCanUI.onDelete += (ingredient) =>
        {
            ThrowTrash(ingredient);
        };
    }

    private void ThrowTrash(IngredientSO ingredient)
    {
        MinusIngredient(ingredient);

        List<MinigameInfo> minigameInfos = CookingManager.GetMinigames();

        for (int i = 0; i < minigameInfos.Count; i++)
        {
            if (ingredient.Equals(minigameInfos[i].reward))
            {
                // ���� �������� ������� �����Ѵ�.
                for (int j = 0; j < minigameInfos[i].ingredients.Length; j++)
                {
                    ThrowTrash(minigameInfos[i].ingredients[j]);
                }
            }
        }

        MemoHandler memoHandler = FindObjectOfType<MemoHandler>();
        memoHandler.RefreshMinigameRecipes();
    }

    private void MinusIngredient(IngredientSO ingredient)
    {
        if(CookingManager.Global.MemoSuccessCountDic.ContainsKey(ingredient))
        {
            CookingManager.Global.MemoSuccessCountDic[ingredient]--;
        }
        else
        {
            CookingManager.Global.MemoSuccessCountDic[ingredient] = 0;
        }
    }
}
