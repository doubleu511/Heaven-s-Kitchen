using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient Scriptable Object", menuName = "ScriptableObjects/Ingredient Scriptable Object")]
public class IngredientSO : ScriptableObject
{
    public int ingredientTranslationId;
    public Sprite ingredientDefaulrSpr;

    [Header("미니게임 스프라이트들")]
    public Sprite[] ClumpedSpr;
}
