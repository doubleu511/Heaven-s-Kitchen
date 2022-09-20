using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Vector3 lookDir;
    public bool isMove = false;

    [Header("Joystick")]
    [SerializeField] FixedJoystick joystick;
    [SerializeField] Button interactiveButton;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        interactiveButton.onClick.AddListener(() =>
        {
            if (currentSelectedObject != null)
            {
                currentSelectedObject.OnInteract();
            }
        });
    }

    private void Update()
    {
        PlayerMove();

        if (isMove)
        {
            lookDir = playerDir;
        }

        SelectObject(out InteractiveObject interactive);

        if (interactive != currentSelectedObject)
        {
            //Init
            interactiveButton.interactable = interactive != null;

            OutlineController beforeOutline = currentSelectedObject?.GetComponent<OutlineController>();
            beforeOutline?.SetOutline(false);

            OutlineController nowOutline = interactive?.GetComponent<OutlineController>();
            nowOutline?.SetOutline(true);

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
        playerDir.x = joystick.Direction.x;
        playerDir.y = joystick.Direction.y;

#if UNITY_EDITOR
        if (playerDir.sqrMagnitude <= 0)
        {
            playerDir.x = Input.GetAxisRaw("Horizontal"); // TO DO : Joystick System
            playerDir.y = Input.GetAxisRaw("Vertical");
        }
#endif


        playerDir = playerDir.normalized;
        isMove = playerDir.sqrMagnitude > 0.01f;

        playerRigid.velocity = playerDir * playerSpeed;
    }

    private void SelectObject(out InteractiveObject _object)
    {
        Vector3 lookPos = transform.position + lookDir;
        RaycastHit2D[] hit = Physics2D.CircleCastAll(lookPos, playerDetectRadius, Vector2.zero, 0, interactiveLayer);

        float savedDistance = Mathf.Infinity;
        GameObject selectedObject = null;

        for (int i = 0; i < hit.Length; i++)
        {
            float distance = (lookPos - hit[i].transform.position).sqrMagnitude;

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
        Gizmos.DrawWireSphere(transform.position + lookDir, playerDetectRadius);
    }
}
