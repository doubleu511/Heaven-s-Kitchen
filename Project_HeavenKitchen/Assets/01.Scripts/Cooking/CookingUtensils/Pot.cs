using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pot : MinigameStarter
{
    private OutlineController outline;

    [SerializeField] Transform potTop;

    protected void Awake()
    {
        outline = GetComponent<OutlineController>();
    }

    public override void OnInteract()
    {
        base.OnInteract();

        potTop.DOKill();
        potTop.DOLocalMoveY(0.5f, 0.75f).OnUpdate(() =>
        {
            outline.RefreshOutline();
        });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            potTop.DOKill();
            potTop.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBounce).OnUpdate(() =>
            {
                outline.RefreshOutline();
            });
        }
    }
}