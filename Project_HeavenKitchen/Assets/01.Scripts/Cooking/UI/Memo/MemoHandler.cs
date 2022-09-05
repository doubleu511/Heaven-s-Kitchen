using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MemoHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] MemoRecipeProcessUI recipeProcessPrefab;
    [SerializeField] MemoIngredientsPanelUI ingredientsPanelPrefab;

    [SerializeField] Transform frontRecipePrefabBoxTrm;
    [SerializeField] Transform backRecipePrefabBoxTrm;
    [SerializeField] Transform ingredientPanelPrefabBoxTrm;

    [SerializeField] RectTransform memoFront;
    [SerializeField] RectTransform memoBack;

    [SerializeField] TextMeshProUGUI memoRecipeNameText;

    [Header("Post It")]
    [SerializeField] Image memoPostIt;
    [SerializeField] TextMeshProUGUI memoPostItIndexText;

    RecipeSO[] recipes = new RecipeSO[1];
    int currentRecipeIndex = 0;

    private MinigameInfo currentTargetMinigame;
    private IngredientSO currentTargetIngredient;

    private void Awake()
    {
        Global.Pool.CreatePool<MemoRecipeProcessUI>(recipeProcessPrefab.gameObject, frontRecipePrefabBoxTrm, 5);
        Global.Pool.CreatePool<MemoIngredientsPanelUI>(ingredientsPanelPrefab.gameObject, ingredientPanelPrefabBoxTrm, 10);
    }

    private void Start()
    {
        CookingManager.Global.OnRecipeAdded += OnRecipeAdded;
        memoRecipeNameText.text = "레시피 없음";
        memoPostIt.gameObject.SetActive(false);
    }

    private void OnRecipeAdded()
    {
        recipes = CookingManager.GetRecipes();

        if (recipes.Length > 0)
        {
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

        bool bFind = false;

        for (int i = 0; i < processUIs.Length; i++)
        {
            processUIs[i].RefreshState(out bool isCompleted);

            if(!isCompleted && !bFind)
            {
                bFind = true;
                currentTargetMinigame = processUIs[i].GetMyInfo();
                SearchNavIngredient();
            }
        }

        if(!bFind)
        {
            currentTargetMinigame = null;
            currentTargetIngredient = null;
        }
    }

    public void SearchNavIngredient()
    {
        if (currentTargetMinigame == null) return;

        MinigameStarter selectedUtensils = CookingManager.Global.TargetNavDic[currentTargetMinigame.minigameNameTranslationId];

        if (selectedUtensils.utensilsInventories.Count == 0)
        {
            selectedUtensils.RefreshInventory();
        }

        for (int i = 0; i < selectedUtensils.utensilsInventories.Count; i++)
        {
            if (selectedUtensils.utensilsInventories[i].minigameId == currentTargetMinigame.minigameNameTranslationId) // 내 미니게임의 주방기구 인벤토리의 맞는 인덱스를 찾는다.
            {
                for (int j = 0; j < selectedUtensils.utensilsInventories[i].ingredients.Length; j++)
                {
                    if(selectedUtensils.utensilsInventories[i].ingredients[j] == null)
                    {
                        IngredientSO ingredient = currentTargetMinigame.ingredients[j];

                        if (CookingManager.Player.Inventory.IsItemExist(ingredient))
                        {
                            continue;
                        }
                        else
                        {
                            currentTargetIngredient = ingredient;
                            return;
                        }
                    }
                }
            }
        }

        currentTargetIngredient = null;
    }

    public void GetNavInfo(out MinigameInfo targetInfo, out IngredientSO targetIngredient)
    {
        targetInfo = currentTargetMinigame;
        targetIngredient = currentTargetIngredient;
    }
}
