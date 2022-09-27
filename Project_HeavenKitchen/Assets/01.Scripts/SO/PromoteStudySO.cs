using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PromoteStudy
{
    public int studyTranslationId;
    public int studyLoreTranslationId;
    public Sprite studySprite;

    public int gainStatValue;
    public int goldCost;
    public int stressValue;
}

[CreateAssetMenu(fileName = "Promote Study Scriptable Object", menuName = "ScriptableObjects/Promote Study Scriptable Object")]
public class PromoteStudySO : ScriptableObject
{
    public int statTranslationId;
    public Define.StatType studyType;
    public PromoteStudy[] studys;
}