using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Global;
    public static PlayerController Player;
    public static MinigameHandler Minigame;
    public static UtensilsUIHandler UtensilsUI;
    public static CounterHandler Counter;

    public MinigameStarter CurrentUtensils;
    public DragAndDropContainer DragAndDropContainer;
    public Material SelectedObejctMat;

    public Dictionary<IngredientSO, int> MemoSuccessCountDic = new Dictionary<IngredientSO, int>();
    public Dictionary<int, MinigameStarter> TargetNavDic = new Dictionary<int, MinigameStarter>();

    private MinigameStarter[] AllUtensils;
    private RecipeSO[] CurrentRecipes = new RecipeSO[1];
    public event Action OnRecipeAdded;

    private List<MinigameInfo> CurrentMinigames = new List<MinigameInfo>();

    public RecipeSO[] canAppearRecipes; // 테스트로 일단 너두고 나중에 필요할때 덮어쓴다.
    public Dictionary<int, RecipeSO> RecipeIDPairDic = new Dictionary<int, RecipeSO>();

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        AllUtensils = FindObjectsOfType<MinigameStarter>();
        Player = FindObjectOfType<PlayerController>(true);
        Minigame = FindObjectOfType<MinigameHandler>();
        UtensilsUI = FindObjectOfType<UtensilsUIHandler>();
        Counter = FindObjectOfType<CounterHandler>();

        for (int i = 0; i < canAppearRecipes.Length; i++)
        {
            RecipeIDPairDic[canAppearRecipes[i].recipeNameTranslationId] = canAppearRecipes[i];
        }
    }

    public static void SetRecipes(RecipeSO recipe)
    {
        Global.MemoSuccessCountDic.Clear();
        Global.TargetNavDic.Clear();
        Global.CurrentRecipes = new RecipeSO[1];
        Global.CurrentRecipes[0] = recipe;

        Global.CurrentMinigames.Clear();
        for (int i = 0; i < recipe.recipe.Length; i++)
        {
            Global.CurrentMinigames.Add(recipe.recipe[i]);
        }
        Global.CurrentMinigames = Global.CurrentMinigames.Distinct().ToList();

        SetTargetNavDic();
        Global.OnRecipeAdded?.Invoke();
    }

    public static void SetRecipes(RecipeSO[] recipe)
    {
        Global.MemoSuccessCountDic.Clear();
        Global.TargetNavDic.Clear();
        Global.CurrentRecipes = recipe;

        Global.CurrentMinigames.Clear();
        for (int i = 0; i < recipe.Length; i++)
        {
            for (int j = 0; j < recipe[i].recipe.Length; j++)
            {
                Global.CurrentMinigames.Add(recipe[i].recipe[j]);
            }
        }
        Global.CurrentMinigames = Global.CurrentMinigames.Distinct().ToList();

        SetTargetNavDic();
        Global.OnRecipeAdded?.Invoke();
    }

    private static void SetTargetNavDic()
    {
        for (int i = 0; i < Global.CurrentMinigames.Count; i++)
        {
            for (int j = 0; j < Global.AllUtensils.Length; j++)
            {
                for (int k = 0; k < Global.AllUtensils[j].cookingUtensilsSO.canPlayMinigameTypes.Length; k++)
                {
                    // 만약, 레시피의 미니게임이 어느 주방기구 하나와 같다면
                    if (Global.CurrentMinigames[i].minigameType == Global.AllUtensils[j].cookingUtensilsSO.canPlayMinigameTypes[k])
                    {
                        Global.TargetNavDic[Global.CurrentMinigames[i].minigameNameTranslationId] = Global.AllUtensils[j];
                    }
                }
            }
        }
    }

    public static RecipeSO[] GetRecipes()
    {
        return Global.CurrentRecipes;
    }

    public static List<MinigameInfo> GetMinigames()
    {
        return Global.CurrentMinigames;
    }
}
