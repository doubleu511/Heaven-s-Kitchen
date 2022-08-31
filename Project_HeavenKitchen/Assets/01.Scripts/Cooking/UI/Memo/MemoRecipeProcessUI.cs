using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MemoRecipeProcessUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI minigameName;
    [SerializeField] Image minigameIcon;
    [SerializeField] Image checkBox;

    [SerializeField] Transform ingredientGridTrm;
    private List<MemoIngredientsPanelUI> ingredientsPanels = new List<MemoIngredientsPanelUI>();

    [SerializeField] TextMeshProUGUI recipeIngredients;

    private readonly int UPGRADE_COUNT = 2;

    MinigameInfo myInfo;

    public void Init(MinigameInfo info)
    {
        myInfo = info;

        // 업그레이드
        recipeIngredients.gameObject.SetActive(UPGRADE_COUNT == 1);
        ingredientGridTrm.gameObject.SetActive(UPGRADE_COUNT == 2);
        switch (UPGRADE_COUNT)
        {
            case 0:
                break;
            case 1:
                UpgradeFirst();
                break;
            case 2:
                UpgradeSecond();
                break;
        }

        minigameName.fontStyle = FontStyles.Normal;
        recipeIngredients.fontStyle = FontStyles.Normal;

        minigameName.text = TranslationManager.Instance.GetLangDialog(info.minigameNameTranslationId);
        minigameIcon.sprite = CookingManager.Global.TargetNavDic[myInfo.minigameNameTranslationId].cookingUtensilsSO.defaultMapSprite;
    }

    private void UpgradeFirst()
    {
        string ingredientsStr = "";

        for (int i = 0; i < myInfo.ingredients.Length; i++)
        {
            ingredientsStr += TranslationManager.Instance.GetLangDialog(myInfo.ingredients[i].ingredientTranslationId);
            if (i < myInfo.ingredients.Length - 1)
            {
                ingredientsStr += ", ";
            }
        }

        string finalValue = $"({ingredientsStr})";

        recipeIngredients.text = finalValue;
    }

    private void UpgradeSecond()
    {
        ingredientsPanels.Clear();

        for (int i = 0; i < ingredientGridTrm.childCount; i++)
        {
            ingredientGridTrm.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < myInfo.ingredients.Length; i++)
        {
            MemoIngredientsPanelUI ingredientsPanel = Global.Pool.GetItem<MemoIngredientsPanelUI>();
            ingredientsPanel.Init(myInfo.ingredients[i]);
            ingredientsPanel.transform.SetParent(ingredientGridTrm);
            ingredientsPanel.transform.SetAsLastSibling();
            ingredientsPanels.Add(ingredientsPanel);
        }
    }

    public void RefreshState(out bool isCompleted)
    {
        bool value = false;
        
        if(CookingManager.Global.MemoSuccessCountDic.ContainsKey(myInfo.reward))
        {
            if(CookingManager.Global.MemoSuccessCountDic[myInfo.reward] > 0)
            {
                value = true;
            }
        }

        isCompleted = value;

        checkBox.gameObject.SetActive(value);

        minigameName.color = value ? new Color32(119, 110, 109, 255) : new Color32(0, 0, 0, 255);
        minigameName.fontStyle = value ? FontStyles.Strikethrough : FontStyles.Normal;

        switch (UPGRADE_COUNT)
        {
            case 0:
                break;
            case 1:
                recipeIngredients.color = value ? new Color32(119, 110, 109, 255) : new Color32(0, 0, 0, 255);
                recipeIngredients.fontStyle = value ? FontStyles.Strikethrough : FontStyles.Normal;
                break;
            case 2:
                {
                    for (int i = 0; i < myInfo.ingredients.Length; i++)
                    {
                        ingredientsPanels[i].SetCheckBox(false);
                    }

                    MinigameStarter selectedUtensils = CookingManager.Global.TargetNavDic[myInfo.minigameNameTranslationId];

                    for(int i =0; i<selectedUtensils.utensilsInventories.Count;i++)
                    {
                        if(selectedUtensils.utensilsInventories[i].minigameId == myInfo.minigameNameTranslationId) // 내 미니게임의 주방기구 인벤토리의 맞는 인덱스를 찾는다.
                        {
                            for (int j = 0; j < myInfo.ingredients.Length; j++)
                            {
                                for (int k = 0; k < selectedUtensils.utensilsInventories[i].ingredients.Length; k++)
                                {
                                    if (myInfo.ingredients[j] == selectedUtensils.utensilsInventories[i].ingredients[k])
                                    {
                                        ingredientsPanels[j].SetCheckBox(true);
                                    }
                                }
                            }
                        }
                    }
                }
                break;
        }
    }

    public MinigameInfo GetMyInfo()
    {
        return myInfo;
    }
}
