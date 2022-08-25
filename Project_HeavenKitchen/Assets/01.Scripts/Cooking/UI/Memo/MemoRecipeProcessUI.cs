using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoRecipeProcessUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI minigameName;
    [SerializeField] TextMeshProUGUI recipeIngredients;

    public void Init(MinigameInfo info)
    {
        minigameName.text = TranslationManager.Instance.GetLangDialog(info.minigameNameTranslationId);

        string ingredientsStr = "";

        for (int i = 0; i < info.ingredients.Length; i++)
        {
            ingredientsStr += TranslationManager.Instance.GetLangDialog(info.ingredients[i].ingredientTranslationId);
            if(i < info.ingredients.Length - 1)
            {
                ingredientsStr += ", ";
            }
        }

        string finalValue = $"({ingredientsStr})";

        recipeIngredients.text = finalValue;
    }

    public void SetStrikethrough(bool value)
    {
        minigameName.fontStyle = value ? FontStyles.Strikethrough : FontStyles.Normal;
        recipeIngredients.fontStyle = value ? FontStyles.Strikethrough : FontStyles.Normal;
    }
}
