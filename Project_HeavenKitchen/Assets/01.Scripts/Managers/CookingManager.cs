using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Global;
    public static PlayerController Player;

    public MinigameStarter CurrentUtensils;
    public DragAndDropContainer DragAndDropContainer;
    public Material SelectedObejctMat;

    public Dictionary<IngredientSO, int> MemoSuccessCountDic;

    private RecipeSO[] CurrentRecipes = new RecipeSO[1];
    [SerializeField] RecipeSO testRecipes; // Å×½ºÆ®

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        Player = FindObjectOfType<PlayerController>(true);
        AddRecipes(testRecipes);
    }

    public static void AddRecipes(RecipeSO recipe)
    {
        Global.CurrentRecipes[0] = recipe;
    }

    public static void AddRecipes(RecipeSO[] recipe)
    {
        Global.CurrentRecipes = recipe;
    }

    public static RecipeSO[] GetRecipes()
    {
        return Global.CurrentRecipes;
    }
}
