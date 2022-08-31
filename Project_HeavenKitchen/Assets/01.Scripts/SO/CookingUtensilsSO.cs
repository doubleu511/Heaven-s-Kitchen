using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingUtensils Scriptable Object", menuName = "ScriptableObjects/CookingUtensils Scriptable Object")]
public class CookingUtensilsSO : ScriptableObject
{
    public int cookingUtensilsTranslationId;
    public Define.CookingTool cookingUtensilsType;
    public SpriteSkinSO minigameBGSpr;
    public Define.MinigameType[] canPlayMinigameTypes;
    public Sprite defaultMapSprite;
}