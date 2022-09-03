using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class IngredientTabUI : MonoBehaviour
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

    public void InventoryClear(Transform moveTo) // �ƿ� �κ��丮 ��ü�� ������.
    {
        for(int i = ingredientInventoryTrm.childCount - 1; i >= 0; i--)
        {
            ingredientInventoryTrm.GetChild(i).gameObject.SetActive(false);
            ingredientInventoryTrm.GetChild(i).SetParent(moveTo, true);
        }
    }

    public void InventoryClean() // �κ��丮 �ȿ��ִ� �������� ������.
    {
        IngredientInventoryUI[] ingredientInventories = ingredientInventoryTrm.GetComponentsInChildren<IngredientInventoryUI>();

        for(int i =0; i<ingredientInventories.Length;i++)
        {
            ingredientInventories[i].CleanInventory();
        }
    }
}
