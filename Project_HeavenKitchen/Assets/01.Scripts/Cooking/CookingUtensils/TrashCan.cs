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

    protected override void Awake()
    {
        base.Awake();
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
        MinusAllIngredients(ingredient);
    }

    private void MinusAllIngredients(IngredientSO ingredient)
    {
        MinusIngredient(ingredient);

        List<MinigameInfo> minigameInfos = CookingManager.GetMinigames();

        for (int i = 0; i < minigameInfos.Count; i++)
        {
            if (ingredient.Equals(minigameInfos[i].reward))
            {
                // 버린 아이템이 리워드로 존재한다.
                for (int j = 0; j < minigameInfos[i].ingredients.Length; j++)
                {
                    MinusAllIngredients(minigameInfos[i].ingredients[j]);
                }
            }
        }

        MemoHandler memoHandler = FindObjectOfType<MemoHandler>();
        memoHandler.RefreshMinigameRecipes();
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

        trashImage.rectTransform.anchoredPosition = new Vector2(0, 230);
        trashImage.rectTransform.sizeDelta = new Vector2(500, 500);


        // Play Animation
        float randomX = Random.Range(-118f, 107f);
        float randomY = Random.Range(-50f, -126f);
        float randomSize = Random.Range(300f, 400f);
        float randomRot = Random.Range(720f - 90f, 720f + 90f);

        trashImage.rectTransform.DOAnchorPos(new Vector2(randomX, randomY), 1.5f);
        trashImage.rectTransform.DOSizeDelta(new Vector2(randomSize, randomSize), 1.5f);
        trashImage.rectTransform.DORotate(new Vector3(0, 0, randomRot), 1.5f);
        trashImage.DOColor(Color.gray, 1.5f);
    }

    private void MinusIngredient(IngredientSO ingredient)
    {
        if(CookingManager.Global.MemoSuccessCountDic.ContainsKey(ingredient))
        {
            CookingManager.Global.MemoSuccessCountDic[ingredient]--;
        }
        else
        {
            CookingManager.Global.MemoSuccessCountDic[ingredient] = 0;
        }
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
