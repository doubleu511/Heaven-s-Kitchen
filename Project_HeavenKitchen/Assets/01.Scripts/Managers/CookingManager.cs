using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Global;
    public static PlayerController Player;

    private RecipeSO[] CurrentRecipes = new RecipeSO[1];
    [SerializeField] RecipeSO testRecipes; // �׽�Ʈ

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
