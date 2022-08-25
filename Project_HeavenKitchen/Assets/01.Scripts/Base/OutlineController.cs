using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public float outlineSize = 1f;
    public int sortingOrder = 0;

    private List<SpriteRenderer> attachedSpriteRenderers = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, SpriteRenderer> spriteRendererPairs = new Dictionary<SpriteRenderer, SpriteRenderer>();

    void Start()
    {
        foreach (SpriteRenderer item in GetComponentsInChildren<SpriteRenderer>(true))
        {
            AddOutline(item);
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
        outlineSprite.material.SetColor("_Color", Color.yellow);
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
        }
    }
}