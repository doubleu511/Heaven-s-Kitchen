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
        for(int i = ingredientInventoryTrm.childCount - 1; i >= 0; i--)
        {
            ingredientInventoryTrm.GetChild(i).gameObject.SetActive(false);
            ingredientInventoryTrm.GetChild(i).SetParent(moveTo, true);
        }
    }
}
