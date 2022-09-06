using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrashCan : MinigameStarter
{
    private OutlineController outline;

    [SerializeField] Transform trashCap;
    [SerializeField] Transform trashPanel;

    public DeleteDragableUI trashCanUI;

    protected void Awake()
    {
        outline = GetComponent<OutlineController>();
    }

    protected override void Start()
    {
        trashCanUI.onDelete += (ingredient) =>
        {
            ThrowTrash(ingredient);
        };
    }

    public override void OnInteract()
    {
        base.OnInteract();

        trashCap.DOKill();
        trashCap.DORotate(new Vector3(0, 0, -70), 0.5f).OnUpdate(() =>
          {
              outline.RefreshOutline();
          });
    }

    private void ThrowTrash(IngredientSO ingredient)
    {
        TrashAnimation(ingredient);
        CookingManager.Global.MinusAllIngredients(ingredient);
    }

    private void TrashAnimation(IngredientSO ingredient)
    {
        // Init
        GameObject trashObject = new GameObject("Trash");
        trashObject.transform.SetParent(trashPanel, false);

        trashObject.AddComponent(typeof(CanvasRenderer));
        Image trashImage = trashObject.AddComponent(typeof(Image)) as Image;
        trashImage.raycastTarget = false;
        trashImage.sprite = ingredient.ingredientDefaulrSpr;

        float sizeScale = ingredient.ingredientDefaulrSpr.rect.width / 512;

        trashImage.rectTransform.anchoredPosition = new Vector2(0, 230);
        trashImage.rectTransform.sizeDelta = new Vector2(500 * sizeScale, 500 * sizeScale);


        // Play Animation
        float randomX = Random.Range(-118f, 107f);
        float randomY = Random.Range(-50f, -126f);
        float randomSize = Random.Range(300f * sizeScale, 400f * sizeScale);
        float randomRot = Random.Range(720f - 90f, 720f + 90f);

        trashImage.rectTransform.DOAnchorPos(new Vector2(randomX, randomY), 1.5f);
        trashImage.rectTransform.DOSizeDelta(new Vector2(randomSize, randomSize), 1.5f);
        trashImage.rectTransform.DORotate(new Vector3(0, 0, randomRot), 1.5f);
        trashImage.DOColor(Color.gray, 1.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            trashCap.DOKill();
            trashCap.DORotate(Vector3.zero, 0.5f).OnUpdate(() =>
            {
                outline.RefreshOutline();
            });
        }
    }
}
