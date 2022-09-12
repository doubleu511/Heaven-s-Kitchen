using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    public bool startAwake = false;
    public bool isLoop = false;
    public bool ignoreTimeScale = false;
    public float delayTime = 0.5f;

    [HideInInspector] public bool refreshNativeSizeEveryFrame = false;

    private bool isPlaying = false;
    private bool isForceStopped = false;

    [Header("Sprites")]
    public Sprite[] sprites;

    private RawImage rawImage;
    private Image image;
    private SpriteRenderer sr;
    private Coroutine animCoroutine;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        image = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();

        if(sprites.Length == 0)
        {
            Debug.LogError(gameObject.name + " : 스프라이트가 할당되지 않았습니다.");
            Destroy(this);
            return;
        }
        else if (!image && !sr && !rawImage)
        {
            Debug.LogError(gameObject.name + " : SpriteRenderer또는 (Raw)Image를 불러오지 못했습니다.");
            Destroy(this);
            return;
        }

        if(startAwake)
        {
            StartAnimation();
        }
    }

    public void StartAnimation()
    {
        if(animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
            animCoroutine = null;

            if(image)
            {
                image.sprite = sprites[0];
            }
            else if (sr)
            {
                sr.sprite = sprites[0];
            }
            else if(rawImage)
            {
                rawImage.texture = sprites[0].texture;
            }
        }

        isPlaying = true;
        animCoroutine = StartCoroutine(Animation());
    }

    public void StopAnimation()
    {
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
            animCoroutine = null;
        }
        isPlaying = false;
    }

    private IEnumerator Animation()
    {
        do
        {
            int index = 0;

            while (index < sprites.Length)
            {
                if (image)
                {
                    image.sprite = sprites[index];
                    if (refreshNativeSizeEveryFrame)
                    {
                        image.SetNativeSize();
                    }
                }
                else if (sr)
                {
                    sr.sprite = sprites[index];
                }
                else if (rawImage)
                {
                    rawImage.texture = sprites[index].texture;
                }

                if (ignoreTimeScale)
                {
                    yield return new WaitForSecondsRealtime(delayTime);
                }
                else
                {
                    yield return new WaitForSeconds(delayTime);
                }
                index++;
            }
        }
        while (isLoop);
    }

    public void SetSprites(Sprite[] _sprites)
    {
        sprites = _sprites;
    }

    private void OnDisable()
    {
        if(isPlaying)
        {
            isForceStopped = true;
        }
    }

    private void OnEnable()
    {
        if(isForceStopped)
        {
            isForceStopped = false;
            StartAnimation();
        }
    }
}
