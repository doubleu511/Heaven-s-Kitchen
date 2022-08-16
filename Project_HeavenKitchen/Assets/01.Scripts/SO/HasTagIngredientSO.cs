using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tag Ingredient Scriptable Object", menuName = "ScriptableObjects/Tag Ingredient Scriptable Object")]
public class HasTagIngredientSO : IngredientSO
{
    [Header("디폴트 스프라이트 태그")]
    public Vector2 tagAnchorPos; // 보통 기준 포스 (512 기준)
    public Sprite[] translationTagSpr;

    public void CreateTag(Image myImage)
    {
        if(translationTagSpr.Length == 0)
        {
            Debug.LogWarning("이 재료는 이름표가 없습니다.");
            return;
        }

        GameObject tagObject = new GameObject("Tag");
        tagObject.transform.SetParent(myImage.transform, false);

        tagObject.AddComponent(typeof(CanvasRenderer));
        Image tagImage = tagObject.AddComponent(typeof(Image)) as Image;
        tagImage.raycastTarget = false;

        Vector2 correctionPos = (tagAnchorPos * myImage.rectTransform.sizeDelta.x) / 512; // 사이즈에 맞춰서 조절
        tagImage.rectTransform.anchoredPosition = correctionPos;

        Vector2 myImgSize = myImage.rectTransform.sizeDelta;
        tagImage.rectTransform.sizeDelta = myImgSize / 8; // 8 배 정도 작더라고 (나중에 512 말고 다른거에 이름표가 필요하다면 바꿀것.)

        tagImage.sprite = translationTagSpr[TranslationManager.Instance.curLangIndex]; // 번역맞춰서 변경
    }
}