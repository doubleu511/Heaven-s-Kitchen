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
    public int repeatCount;
    public IngredientSO[] ingredients;
    public IngredientSO reward;

    public IngredientSO FindIngredientsSprites(string name, out Sprite[] sprites)
    {
        IngredientSO ingredient = null;
        sprites = null;
        int count = 0;

        for (int i = 0; i < ingredients.Length; i++)
        {
            if (ingredients[i].FindSprites(name, out Sprite[] _savedSprs))
            {
                ingredient = ingredients[i];
                sprites = _savedSprs;
                count++;
            }
        }

        if (count > 1)
        {
            Debug.LogWarning("두 개 이상의 다른 재료가 똑같은 음식 스프라이트 이름을 가져옵니다. 그중 하나만 할당합니다.");
        }

        return ingredient;
    }
}