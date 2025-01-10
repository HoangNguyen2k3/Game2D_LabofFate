using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    PlayerController instance;
    public bool playTest = false;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float dashSpeed = 24f;
    [SerializeField] private float dashDuration= 0.3f;
    [SerializeField] private float dashCooldown = 0.7f;
   // [SerializeField] private TrailRenderer playerTrailRenderer;
    public bool isDashing {get; private set;} = false;
    private bool canDash = true;
    private Animator animator;
    private SlashManagerCombo slashManagerCombo;
    private bool isAttacking => slashManagerCombo.isAttacking.Value;
    private Vector3 otherPos;
    private PlayerHealth health;
    private KnockBack knockBack;
    public string DirectionStr {get; private set;} = "Down";
    public bool stopMovingInstruction = false;

    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;

     private void Awake()
    {
        //   base.Awake();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slashManagerCombo = GetComponent<SlashManagerCombo>();
        health = GetComponent<PlayerHealth>();
        knockBack = GetComponent<KnockBack>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
        }
        else
        {
            vc.Priority = 0;
        }
    }
    private void Update()
    {
        if (health.isDead.Value) return;

        UpdateDirectionStr();
        if (stopMovingInstruction) {
            rb.velocity = Vector2.zero;
            return; }
        if (isDashing || isAttacking) return;
        
        if (IsOwner || playTest)
        {
           
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();

            animator.SetBool("isMoving", moveInput != Vector2.zero);

            if ((Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.LeftShift)) && canDash)
            {
                StartCoroutine(Dash());
            }
            SyncPlayerPosServerRpc(transform.position);
        }
        else
        {
            transform.position = otherPos;
        }
    }

    private void FixedUpdate()
    {
        
        if (IsOwner)
        {
            if (stopMovingInstruction
                ) {
                rb.velocity = Vector2.zero;
                moveInput = Vector2.zero; return; }
            if (isDashing) return;
            if (knockBack.GetKnockBack) return;
            if (isAttacking)
            {
                rb.velocity *= 0.8f * 50 * Time.deltaTime;
                return;
            }
            rb.velocity = new Vector2 (moveInput.x * speed, moveInput.y * speed);
        }
    }

    private IEnumerator Dash()
    {
        Vector2 dashDirection = moveInput;
        canDash = false;
        isDashing = true;
        health.canTakeDamage = false;
        
        rb.velocity = new Vector2 (dashDirection.x * dashSpeed, dashDirection.y * dashSpeed);
     //   ToggleTrailRendererServerRpc(true);
        knockBack.canBeKnockback = false;
        yield return new WaitForSeconds(dashDuration);
        knockBack.canBeKnockback = true;
        // ToggleTrailRendererServerRpc(false); 
        isDashing = false;
        health.canTakeDamage = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;   
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    private void UpdateDirectionStr()
    {
        if (moveInput == Vector2.zero) return;
        
        // Get angle between (0,1) and moveInput to determine Sprite directions
        float inputAngle = Vector2.SignedAngle(Vector2.right, moveInput); 

        if ((-45f < inputAngle && inputAngle < 45f) || -135f > inputAngle || inputAngle > 135f) 
        {
            DirectionStr = "Left";
            return;
        }

        if (45f <= inputAngle && inputAngle <= 135f) 
        {
            DirectionStr = "Up";
            return;
        }

        DirectionStr = "Down";
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncPlayerPosServerRpc(Vector3 pos)
    {
        otherPos = pos;
    }

/*    [ServerRpc(RequireOwnership = false)]
    private void ToggleTrailRendererServerRpc(bool isActive)
    {
        ToggleTrailRendererClientRpc(isActive);
    }*/

/*    [ClientRpc]
    private void ToggleTrailRendererClientRpc(bool isActive)
    {
        playerTrailRenderer.emitting = isActive;
    }*/
}
