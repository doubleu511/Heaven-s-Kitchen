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
    [SerializeField] MemoDishProcessUI dishProcessUI;

    [Header("Post It")]
    [SerializeField] Image memoPostIt;
    [SerializeField] TextMeshProUGUI memoPostItIndexText;

    RecipeSO[] recipes = new RecipeSO[0];
    int currentRecipeIndex = 0;

    // 네비게이션용 변수들!
    private MinigameInfo currentTargetMinigame;    // 현재 가야하는 미니게임 (NULL이면 카운터)
    private IngredientSO currentTargetIngredient;  // 현재 가져가야하는 재료 (NULL이면 현재 주방기구)

    private void Awake()
    {
        Global.Pool.CreatePool<MemoRecipeProcessUI>(recipeProcessPrefab.gameObject, frontRecipePrefabBoxTrm, 5);
        Global.Pool.CreatePool<MemoIngredientsPanelUI>(ingredientsPanelPrefab.gameObject, ingredientPanelPrefabBoxTrm, 10);
    }

    private void Start()
    {
        // 처음 시작할땐 레시피가 없지.
        CookingManager.Global.OnRecipeAdded += OnRecipeAdded;
        memoRecipeNameText.text = "레시피 없음";
        memoPostIt.gameObject.SetActive(false);
    }

    private void OnRecipeAdded()
    {
        // 레시피가 추가되었을 때

        recipes = CookingManager.GetRecipes();
        currentRecipeIndex = 0;

        if (recipes.Length > 0)
        {
            FrontInit(currentRecipeIndex);
            memoPostItIndexText.text = $"{currentRecipeIndex + 1}";

            if (recipes.Length > 1)
            {
                // 만약 레시피가 두개 이상이면, 뒷쪽 사이즈도 표시해준다.
                int nextRecipeIndex = (currentRecipeIndex + 1) % recipes.Length;
                BackInit(nextRecipeIndex);
            }

            memoBack.gameObject.SetActive(recipes.Length > 1);
            memoPostIt.gameObject.SetActive(recipes.Length > 1);
        }
        else
        {
            // 레시피가 추가된게 아니고 사라진거라면... 메모지를 초기화한다.

            for (int i = 1; i < frontRecipePrefabBoxTrm.childCount; i++)
            {
                frontRecipePrefabBoxTrm.GetChild(i).gameObject.SetActive(false);
            }

            // ContentSizeFitter를 강제 새로고침한다.
            UtilClass.ForceRefreshSize(transform);

            memoRecipeNameText.text = "레시피 없음";
            memoBack.gameObject.SetActive(false);
            memoPostIt.gameObject.SetActive(false);

            RefreshMinigameRecipes();
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

        if (currentRecipe.recipe.Length > 0)
        {
            for (int i = 0; i < currentRecipe.recipe.Length; i++)
            {
                MemoRecipeProcessUI recipe = Global.Pool.GetItem<MemoRecipeProcessUI>();
                recipe.transform.SetParent(frontRecipePrefabBoxTrm);
                recipe.transform.SetAsLastSibling();

                recipe.Init(currentRecipe.recipe[i]);
            }

            // ContentSizeFitter를 강제 새로고침한다.
            UtilClass.ForceRefreshSize(transform);
            RefreshMinigameRecipes();
        }
        else  // 가끔 레시피가 없는 주문을 할경우 이쪽으로 간다.
        {
            if (!CookingManager.Player.Inventory.IsItemExist(currentRecipe.foodIngredient))
            {
                currentTargetIngredient = currentRecipe.foodIngredient;
            }
            else
            {
                currentTargetIngredient = null;
            }
        }

        dishProcessUI.Refresh(currentRecipe);
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
        UtilClass.ForceRefreshSize(transform);
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
            Global.Sound.Play("SFX/CookingScene/paperflip", Define.Sound.Effect);
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

        if(!bFind || processUIs.Length == 0)
        {
            currentTargetMinigame = null;
            currentTargetIngredient = null;
        }
    }

    public void SearchNavIngredient()
    {
        if (currentTargetMinigame != null)
        {
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
                        if (selectedUtensils.utensilsInventories[i].ingredients[j] == null)
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
        else // 가끔 레시피가 없는 주문을 할경우 이쪽으로 간다.
        {
            if (recipes.Length > 0)
            {
                RecipeSO currentRecipe = recipes[currentRecipeIndex];

                if (!CookingManager.Player.Inventory.IsItemExist(currentRecipe.foodIngredient))
                {
                    currentTargetIngredient = currentRecipe.foodIngredient;
                }
                else
                {
                    currentTargetIngredient = null;
                }
            }
            else
            {
                currentTargetMinigame = null;
                currentTargetIngredient = null;
            }
        }
    }

    public RecipeSO GetCurrentRecipe()
    {
        if (currentRecipeIndex < recipes.Length)
        {
            return recipes[currentRecipeIndex];
        }

        return null;
    }

    public void GetNavInfo(out MinigameInfo targetInfo, out IngredientSO targetIngredient)
    {
        targetInfo = currentTargetMinigame;
        targetIngredient = currentTargetIngredient;
    }
}
