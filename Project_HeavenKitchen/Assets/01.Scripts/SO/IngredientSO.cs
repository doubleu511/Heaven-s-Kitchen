using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Ingredient Scriptable Object", menuName = "ScriptableObjects/Ingredient Scriptable Object")]
public class IngredientSO : ScriptableObject
{
    public int ingredientTranslationId;
    public int ingredientLoreId;
    public Sprite ingredientDefaulrSpr;
    public Sprite ingredientMiniSpr;

    [Header("미니게임 스프라이트들")]
    public List<IngredientMinigameSprite> ingredientMinigameSprite = new List<IngredientMinigameSprite>();

    [Header("재료 파티클")]
    public ParticleSystem particlePrefab; // TO DO : 나중에 이것도 스프라이트처럼 나눠야할수도?

    [Header("재료 속성")]
    public bool isFood;
    public bool isHot;

    public bool FindSprites(string name, out Sprite[] sprites)
    {
        IngredientMinigameSprite nameOfSprites = ingredientMinigameSprite.Find(x => x.spritesName == name);
        if(nameOfSprites != null)
        {
            sprites = nameOfSprites.sprites;
            return true;
        }

        sprites = null;
        return false;
    }
}

[System.Serializable]
public class IngredientMinigameSprite
{
    public string spritesName;
    public Sprite[] sprites;
}