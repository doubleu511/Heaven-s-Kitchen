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

    [Header("�̴ϰ��� ��������Ʈ��")]
    public List<IngredientMinigameSprite> ingredientMinigameSprite = new List<IngredientMinigameSprite>();

    [Header("��� ��ƼŬ")]
    public ParticleSystem particlePrefab; // TO DO : ���߿� �̰͵� ��������Ʈó�� �������Ҽ���?

    [Header("��� �Ӽ�")]
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