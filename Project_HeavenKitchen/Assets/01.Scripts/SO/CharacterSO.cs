using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName ="Character Scriptable Object", menuName = "ScriptableObjects/Character Scriptable Object")]
public class CharacterSO : ScriptableObject
{
    public int characterNameTranslationId;
    public Define.CharacterTextStyle textStyle;
    public Sprite characterPortrait;
    public TMP_FontAsset characterFont;

    public Sprite[] faceBox;
    public Sprite[] clothesBox;
}
