using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_IngredientTab : MonoBehaviour
{
    public Transform ingredientInventoryTrm;
    [SerializeField] TextMeshProUGUI minigameName;
    
    public void InitName(string name)
    {
        minigameName.text = name;
    }

    public void InventoryClear(Transform moveTo)
    {
        for(int i = 0; i < ingredientInventoryTrm.childCount; i++)
        {
            ingredientInventoryTrm.GetChild(i).SetParent(moveTo, true);
        }
    }
}
