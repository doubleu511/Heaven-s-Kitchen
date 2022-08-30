using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Global;
    public static PlayerController Player;

    public MinigameStarter CurrentUtensils;
    public DragAndDropContainer DragAndDropContainer;
    public Material SelectedObejctMat;

    public Dictionary<IngredientSO, int> MemoSuccessCountDic = new Dictionary<IngredientSO, int>();
    public Dictionary<int, MinigameStarter> TargetNavDic = new Dictionary<int, MinigameStarter>();

    private MinigameStarter[] AllUtensils;
    private RecipeSO[] CurrentRecipes = new RecipeSO[1];
    private List<MinigameInfo> CurrentMinigames = new List<MinigameInfo>();
    [SerializeField] RecipeSO[] testRecipes; // 테스트

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        AllUtensils = FindObjectsOfType<MinigameStarter>();
        Player = FindObjectOfType<PlayerController>(true);
        SetRecipes(testRecipes);
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
