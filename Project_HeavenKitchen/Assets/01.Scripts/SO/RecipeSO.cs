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
            Debug.LogWarning("�� �� �̻��� �ٸ� ��ᰡ �Ȱ��� ���� ��������Ʈ �̸��� �����ɴϴ�. ���� �ϳ��� �Ҵ��մϴ�.");
        }

        return ingredient;
    }
}