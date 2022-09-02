using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public float outlineSize = 3f;
    public int sortingOrder = 0;
    [SerializeField] SpriteRenderer[] ignoreRenderers;

    private List<SpriteRenderer> attachedSpriteRenderers = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, SpriteRenderer> spriteRendererPairs = new Dictionary<SpriteRenderer, SpriteRenderer>();

    void Start()
    {
        SpriteRenderer[] allSpriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        for (int i = 0; i < allSpriteRenderers.Length; i++)
        {
            bool flag = true;
            for (int j = 0; j < ignoreRenderers.Length; j++)
            {
                if (allSpriteRenderers[i] == ignoreRenderers[j])
                {
                    flag = false;
                }
            }

            if (flag)
            {
                AddOutline(allSpriteRenderers[i]);
            }
        }
    }

    private void AddOutline(SpriteRenderer sprite)
    {
        GameObject outline = new GameObject("Outline");
        outline.transform.parent = sprite.transform;
        outline.transform.position = sprite.transform.position;

        Vector3 outlineLocalPos = outline.transform.localPosition;
        outlineLocalPos.z = 0.05f;
        outline.transform.localPosition = outlineLocalPos;

        SpriteRenderer outlineSprite = outline.AddComponent<SpriteRenderer>();
        outlineSprite.sprite = sprite.sprite;
        outlineSprite.material = CookingManager.Global.SelectedObejctMat;
        outlineSprite.material.SetFloat("_Thickness", outlineSize);
        outlineSprite.sortingOrder = sortingOrder;

        spriteRendererPairs[outlineSprite] = sprite;
        outlineSprite.gameObject.SetActive(false);

        attachedSpriteRenderers.Add(outlineSprite);
    }

    public void SetOutline(bool value)
    {
        foreach (SpriteRenderer item in attachedSpriteRenderers)
        {
            item.gameObject.SetActive(value);
        }
    }

    public void RefreshOutline()
    {
        foreach (SpriteRenderer item in attachedSpriteRenderers)
        {
            SpriteRenderer originSR = spriteRendererPairs[item];

            item.sprite = originSR.sprite;
            item.transform.position = new Vector3(originSR.transform.position.x, originSR.transform.position.y, item.transform.position.z);
            item.transform.rotation = originSR.transform.rotation;
        }
    }
}