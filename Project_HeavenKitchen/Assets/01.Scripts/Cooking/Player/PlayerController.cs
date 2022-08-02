using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInventory Inventory;

    private Rigidbody2D playerRigid;

    public InteractiveObject currentSelectedObject = null;

    public readonly float playerDefaultSpeed = 4;

    public LayerMask interactiveLayer;
    public float playerDetectRadius = 3;
    public float playerSpeed;
    public Vector3 playerDir;
    public bool isMove = false;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMove();
        SelectObject(out InteractiveObject interactive);

        if (interactive != currentSelectedObject)
        {
            currentSelectedObject = interactive;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(currentSelectedObject != null)
            {
                currentSelectedObject.OnInteract();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 YToZ = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        transform.position = YToZ;
    }

    private void PlayerMove()
    {
        playerDir.x = Input.GetAxisRaw("Horizontal"); // TO DO : Joystick System
        playerDir.y = Input.GetAxisRaw("Vertical");

        playerDir = playerDir.normalized;
        isMove = playerDir.sqrMagnitude > 0.01f;

        playerRigid.velocity = playerDir * playerSpeed;
    }

    private void SelectObject(out InteractiveObject _object)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, playerDetectRadius, Vector2.zero, 0, interactiveLayer);

        float savedDistance = Mathf.Infinity;
        GameObject selectedObject = null;

        for (int i = 0; i < hit.Length; i++)
        {
            float distance = (transform.position - hit[i].transform.position).sqrMagnitude;

            if (savedDistance > distance)
            {
                savedDistance = distance;
                selectedObject = hit[i].collider.gameObject;
            }
        }

        _object = selectedObject?.GetComponent<InteractiveObject>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, playerDetectRadius);
    }
}
