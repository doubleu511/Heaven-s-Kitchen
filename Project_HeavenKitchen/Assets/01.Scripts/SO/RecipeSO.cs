using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe Scriptable Object", menuName = "ScriptableObjects/Recipe Scriptable Object")]
public class RecipeSO : ScriptableObject
{
    public int recipeNameTranslationId;
    public MinigameInfo[] recipe;
}

[System.Serializable]
public class MinigameInfo
{
    public int minigameNameTranslationId;
    public Define.MinigameType minigameType;
    public IngredientSO[] ingredients;
    public IngredientSO reward;
}