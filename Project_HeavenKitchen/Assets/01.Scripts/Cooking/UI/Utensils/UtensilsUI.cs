using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtensilsUI : MonoBehaviour
{
    [SerializeField] Transform circleTrm;
    [SerializeField] GameObject processBar;
    [SerializeField] GameObject processBarValue;

    int ingredientsCount = 0;

    public void InventoryRefresh(List<UtensilsInventory> inventory)
    {
        ingredientsCount = 0;

        for (int i = 0; i < circleTrm.childCount; i++)
        {
            circleTrm.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            for (int j = 0; j < inventory[i].ingredients.Length; j++)
            {
                if (inventory[i].ingredients[j] != null)
                {
                    ingredientsCount++;

                    if (ingredientsCount < 4)
                    {
                        UtensilsCircleInventoryUI circleUI = Global.Pool.GetItem<UtensilsCircleInventoryUI>();
                        circleUI.transform.SetParent(circleTrm);
                        circleUI.transform.SetAsLastSibling();
                        circleUI.Init(inventory[i].ingredients[j]);
                    }
                }
            }
        }

        if(ingredientsCount >= 4)
        {
            UtensilsCircleInventoryUI circleUI = Global.Pool.GetItem<UtensilsCircleInventoryUI>();
            circleUI.transform.SetParent(circleTrm);
            circleUI.transform.SetAsLastSibling();
            circleUI.SetPlus();
        }
    }

    public void HideProcessBar()
    {
        processBar.SetActive(false);
    }

    public void SetProcessBar(float value)
    {
        if(!processBar.activeSelf)
        {
            processBar.SetActive(true);
        }
        processBarValue.transform.localScale = new Vector3(value, 1);
    }
}
