using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guest Scriptable Object", menuName = "ScriptableObjects/Guest Scriptable Object")]
public class GuestSO : ScriptableObject
{
    public int[] guestNameTranslationIds;
    public Sprite guestPortrait;

    public Vector2 faceRectPos;
    public Sprite[] faceBox;

    public int heyTranslationId;
    public int[] canPlayDialogIds;
    public int[] hurryUpTranlationIds;
    public int[] quitTranslationIds;
    public int[] gameOverTranslationIds;
}
