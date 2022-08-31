using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoIngredientsPanelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ingredientName;
    [SerializeField] Image ingredientIcon;
    [SerializeField] Image strikeThrough;

    public void Init(IngredientSO ingredient)
    {
        ingredientName.text = TranslationManager.Instance.GetLangDialog(ingredient.ingredientTranslationId);
        ingredientIcon.sprite = ingredient.ingredientMiniSpr;

        SetCheckBox(false);
    }

    public void SetCheckBox(bool value)
    {
        strikeThrough.gameObject.SetActive(value);
    }
}
