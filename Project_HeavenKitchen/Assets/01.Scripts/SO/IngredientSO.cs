using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient Scriptable Object", menuName = "ScriptableObjects/Ingredient Scriptable Object")]
public class IngredientSO : ScriptableObject
{
    public int ingredientTranslationId;
    public Sprite ingredientDefaulrSpr;

    [Header("�̴ϰ��� ��������Ʈ��")]
    public Sprite[] ClumpedSpr;
}
