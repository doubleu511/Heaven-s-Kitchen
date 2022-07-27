using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character Scriptable Object", menuName = "ScriptableObjects/Character Scriptable Object")]
public class CharacterSO : ScriptableObject
{
    public int characterNameTranslationId;
    public Define.CharacterTextStyle textStyle;

    public Sprite[] faceBox;
    public Sprite[] clothesBox;
}
