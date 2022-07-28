using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigid;

    public readonly float playerDefaultSpeed = 4;

    public float playerSpeed;
    public Vector2 playerDir;
    public bool isMove = false;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerDir.x = Input.GetAxisRaw("Horizontal");
        playerDir.y = Input.GetAxisRaw("Vertical");

        playerDir = playerDir.normalized;
        isMove = playerDir.sqrMagnitude > 0.01f;

        playerRigid.velocity = playerDir * playerSpeed;
    }
}
