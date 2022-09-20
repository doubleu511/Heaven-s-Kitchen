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

    private AudioSource audioSource;
    private bool isOpen = false;

    protected void Awake()
    {
        outline = GetComponent<OutlineController>();
        audioSource = GetComponent<AudioSource>();
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
        if (!isOpen)
        {
            isOpen = true;
            base.OnInteract();

            trashCap.DOKill();
            trashCap.DORotate(new Vector3(0, 0, -70), 0.5f).OnUpdate(() =>
              {
                  outline.RefreshOutline();
              });
            Global.Sound.Play("SFX/Utensils/trashcan_01", audioSource);
        }
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
        float sizeProportion = ingredient.ingredientDefaulrSpr.rect.height / ingredient.ingredientDefaulrSpr.rect.width;

        trashImage.rectTransform.anchoredPosition = new Vector2(0, 230);
        trashImage.rectTransform.sizeDelta = new Vector2(500 * sizeScale, 500 * sizeProportion * sizeScale);

        Global.Sound.Play("SFX/object_falling", Define.Sound.Effect);


        // Play Animation
        float randomX = Random.Range(-118f, 107f);
        float randomY = Random.Range(-50f, -126f);
        float randomSize = Random.Range(300f * sizeScale, 400f * sizeScale);
        float randomRot = Random.Range(720f - 90f, 720f + 90f);

        trashImage.rectTransform.DOAnchorPos(new Vector2(randomX, randomY), 1.5f);
        trashImage.rectTransform.DOSizeDelta(new Vector2(randomSize, randomSize * sizeProportion), 1.5f);
        trashImage.rectTransform.DORotate(new Vector3(0, 0, randomRot), 1.5f);
        trashImage.DOColor(Color.gray, 1.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOpen)
            {
                isOpen = false;
                trashCap.DOKill();
                trashCap.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBounce).OnUpdate(() =>
                {
                    outline.RefreshOutline();
                });
                Global.Sound.Play("SFX/Utensils/trashcan_00", audioSource);
            }
        }
    }
}
