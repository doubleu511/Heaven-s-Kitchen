using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Scriptable Object", menuName = "ScriptableObjects/Skin Scriptable Object")]
public class SpriteSkinSO : ScriptableObject
{
    public Sprite[] skinIndex;
}