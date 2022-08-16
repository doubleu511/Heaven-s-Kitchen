using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tag Ingredient Scriptable Object", menuName = "ScriptableObjects/Tag Ingredient Scriptable Object")]
public class HasTagIngredientSO : IngredientSO
{
    [Header("����Ʈ ��������Ʈ �±�")]
    public Vector2 tagAnchorPos; // ���� ���� ���� (512 ����)
    public Sprite[] translationTagSpr;

    public void CreateTag(Image myImage)
    {
        if(translationTagSpr.Length == 0)
        {
            Debug.LogWarning("�� ���� �̸�ǥ�� �����ϴ�.");
            return;
        }

        GameObject tagObject = new GameObject("Tag");
        tagObject.transform.SetParent(myImage.transform, false);

        tagObject.AddComponent(typeof(CanvasRenderer));
        Image tagImage = tagObject.AddComponent(typeof(Image)) as Image;
        tagImage.raycastTarget = false;

        Vector2 correctionPos = (tagAnchorPos * myImage.rectTransform.sizeDelta.x) / 512; // ����� ���缭 ����
        tagImage.rectTransform.anchoredPosition = correctionPos;

        Vector2 myImgSize = myImage.rectTransform.sizeDelta;
        tagImage.rectTransform.sizeDelta = myImgSize / 8; // 8 �� ���� �۴���� (���߿� 512 ���� �ٸ��ſ� �̸�ǥ�� �ʿ��ϴٸ� �ٲܰ�.)

        tagImage.sprite = translationTagSpr[TranslationManager.Instance.curLangIndex]; // �������缭 ����
    }
}