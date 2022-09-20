using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;

    public Animator basketAnimator;
    public Transform itemParent;

    private void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerController.isMove)
        {
            playerSpriteRenderer.flipX = playerController.playerDir.x < 0;
        }

        playerAnimator.SetFloat("dirX", playerController.playerDir.x);
        playerAnimator.SetFloat("dirY", playerController.playerDir.y);
        playerAnimator.SetFloat("SpeedMultiplier", playerController.playerSpeed / playerController.playerDefaultSpeed); // TO DO : Don't Need Update
        playerAnimator.SetBool("isMove", playerController.isMove);

        basketAnimator.SetFloat("dirX", playerController.playerDir.x);
        basketAnimator.SetBool("isMove", playerController.isMove);
    }

    public void DishTransformEdit(bool value)
    {
        SpriteRenderer basketSR = basketAnimator.GetComponent<SpriteRenderer>();

        basketSR.enabled = !value;
        itemParent.transform.localPosition = value ? new Vector2(0, -0.25f) : Vector2.zero;
    }
}
