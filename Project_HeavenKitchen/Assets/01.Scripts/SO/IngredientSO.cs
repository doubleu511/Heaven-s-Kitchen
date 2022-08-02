using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient Scriptable Object", menuName = "ScriptableObjects/Ingredient Scriptable Object")]
public class IngredientSO : ScriptableObject
{
    public int ingredientTranslationId;
    public Sprite ingredientDefaulrSpr;
    public Sprite ingredientMiniSpr;

    [Header("�̴ϰ��� ��������Ʈ��")]
    public List<IngredientMinigameSprite> ingredientMinigameSprite = new List<IngredientMinigameSprite>();
}

[System.Serializable]
public class IngredientMinigameSprite
{
    public string spritesName;
    public Sprite[] sprites;
}