using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MemoHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] MemoRecipeProcessUI recipeProcessPrefab;
    [SerializeField] Transform frontRecipePrefabBoxTrm;
    [SerializeField] Transform backRecipePrefabBoxTrm;

    [SerializeField] RectTransform memoFront;
    [SerializeField] RectTransform memoBack;

    [SerializeField] TextMeshProUGUI memoRecipeNameText;

    [Header("Post It")]
    [SerializeField] Image memoPostIt;
    [SerializeField] TextMeshProUGUI memoPostItIndexText;

    RecipeSO[] recipes;
    int currentRecipeIndex = 0;

    private void Awake()
    {
        Global.Pool.CreatePool<MemoRecipeProcessUI>(recipeProcessPrefab.gameObject, frontRecipePrefabBoxTrm, 5);
    }

    private void Start()
    {
        recipes = CookingManager.GetRecipes();

        FrontInit(currentRecipeIndex);
        memoPostItIndexText.text = $"{currentRecipeIndex + 1}";

        if (recipes.Length > 1)
        {
            int nextRecipeIndex = (currentRecipeIndex + 1) % recipes.Length;
            BackInit(nextRecipeIndex);
        }

        memoBack.gameObject.SetActive(recipes.Length > 1);
        memoPostIt.gameObject.SetActive(recipes.Length > 1);
    }

    private void FrontInit(int index)
    {
        RecipeSO currentRecipe = recipes[index];

        memoRecipeNameText.text = TranslationManager.Instance.GetLangDialog(currentRecipe.recipeNameTranslationId);

        for (int i = 1; i < frontRecipePrefabBoxTrm.childCount; i++)
        {
            frontRecipePrefabBoxTrm.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentRecipe.recipe.Length; i++)
        {
            MemoRecipeProcessUI recipe = Global.Pool.GetItem<MemoRecipeProcessUI>();
            recipe.transform.SetParent(frontRecipePrefabBoxTrm);
            recipe.transform.SetAsLastSibling();

            recipe.Init(currentRecipe.recipe[i]);
        }

        // ContentSizeFitter를 강제 새로고침한다.
        ContentSizeFitter[] csfs = GetComponentsInChildren<ContentSizeFitter>();
        for(int i =0; i< csfs.Length;i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csfs[i].transform);
        }

        RefreshMinigameRecipes();
    }

    private void BackInit(int index)
    {
        RecipeSO currentRecipe = recipes[index];

        for (int i = 1; i < backRecipePrefabBoxTrm.childCount; i++)
        {
            backRecipePrefabBoxTrm.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentRecipe.recipe.Length; i++)
        {
            MemoRecipeProcessUI recipe = Global.Pool.GetItem<MemoRecipeProcessUI>();
            recipe.transform.SetParent(backRecipePrefabBoxTrm);
            recipe.transform.SetAsLastSibling();

            recipe.Init(currentRecipe.recipe[i]);
        }

        // ContentSizeFitter를 강제 새로고침한다.
        ContentSizeFitter[] csfs = GetComponentsInChildren<ContentSizeFitter>();
        for (int i = 0; i < csfs.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csfs[i].transform);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickMemo();
    }

    private void OnClickMemo()
    {
        if(recipes.Length > 1)
        {
            currentRecipeIndex = (currentRecipeIndex + 1) % recipes.Length;
            FrontInit(currentRecipeIndex);

            memoPostItIndexText.text = $"{currentRecipeIndex + 1}";

            int nextRecipeIndex = (currentRecipeIndex + 1) % recipes.Length;
            BackInit(nextRecipeIndex);

            RefreshMinigameRecipes();
        }
    }

    public void RefreshMinigameRecipes()
    {
        MemoRecipeProcessUI[] processUIs = frontRecipePrefabBoxTrm.GetComponentsInChildren<MemoRecipeProcessUI>();

        for (int i = 0; i < processUIs.Length; i++)
        {
            processUIs[i].RefreshStrikethrough();
        }
    }
}
