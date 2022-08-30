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

    private RecipeSO[] CurrentRecipes = new RecipeSO[1];
    private List<MinigameInfo> CurrentMinigames = new List<MinigameInfo>();
    [SerializeField] RecipeSO[] testRecipes; // Å×½ºÆ®

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        Player = FindObjectOfType<PlayerController>(true);
        SetRecipes(testRecipes);
    }

    public static void SetRecipes(RecipeSO recipe)
    {
        Global.CurrentRecipes = new RecipeSO[1];
        Global.CurrentRecipes[0] = recipe;

        Global.CurrentMinigames.Clear();
        for (int i = 0; i < recipe.recipe.Length; i++)
        {
            Global.CurrentMinigames.Add(recipe.recipe[i]);
        }
        Global.CurrentMinigames = Global.CurrentMinigames.Distinct().ToList();
    }

    public static void SetRecipes(RecipeSO[] recipe)
    {
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
