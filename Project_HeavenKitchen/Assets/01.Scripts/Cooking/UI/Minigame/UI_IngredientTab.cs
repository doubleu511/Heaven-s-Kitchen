using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_IngredientTab : MonoBehaviour
{
    public Transform ingredientInventoryTrm;
    [SerializeField] TextMeshProUGUI minigameName;
    [SerializeField] Button minigameStartBtn;
    
    public void InitName(string name)
    {
        minigameName.text = name;
    }

    public void SetStartAction(Action action)
    {
        minigameStartBtn.onClick.RemoveAllListeners();
        minigameStartBtn.onClick.AddListener(() => {
            if (action != null)
            {
                action.Invoke();
            }
        });
    }

    public void InventoryClear(Transform moveTo) // 아예 인벤토리 자체를 날린다.
    {
        for(int i = ingredientInventoryTrm.childCount - 1; i >= 0; i--)
        {
            ingredientInventoryTrm.GetChild(i).gameObject.SetActive(false);
            ingredientInventoryTrm.GetChild(i).SetParent(moveTo, true);
        }
    }

    public void InventoryClean() // 인벤토리 안에있는 아이템을 날린다.
    {
        UI_IngredientInventory[] ingredientInventories = ingredientInventoryTrm.GetComponentsInChildren<UI_IngredientInventory>();

        for(int i =0; i<ingredientInventories.Length;i++)
        {
            ingredientInventories[i].CleanInventory();
        }
    }
}
