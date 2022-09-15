using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLocalize : MonoBehaviour
{
    public Sprite[] beLocalizedSprite;

    private Image image;
    private SpriteRenderer sr;

    private void Awake()
    {
        image = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();

        if (beLocalizedSprite.Length == 0)
        {
            Debug.LogError(gameObject.name + " : ��������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            Destroy(this);
            return;
        }
        else if (!image && !sr)
        {
            Debug.LogError(gameObject.name + " : SpriteRenderer�Ǵ� Image�� �ҷ����� ���߽��ϴ�.");
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        TranslationManager.Instance.LocalizeChanged += CallBackLocalizeChanged;
        CallBackLocalizeChanged();
    }

    protected virtual void CallBackLocalizeChanged()
    {
        if (image)
        {
            image.sprite = beLocalizedSprite[TranslationManager.Instance.curLangIndex];
        }

        if (sr)
        {
            sr.sprite = beLocalizedSprite[TranslationManager.Instance.curLangIndex];
        }
    }
}
