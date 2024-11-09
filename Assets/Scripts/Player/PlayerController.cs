using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 dashDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private TrailRenderer playerTrailRenderer;
    private bool isDashing = false;
    private bool canDash = false;
    private Animator animator;
    private Vector3 otherPos;
    private PlayerHealth health;
    private KnockBack knockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Update()
    {
        if (health.isDead.Value) return;

        if (IsOwner)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            animator.SetBool("isWalk", moveInput != Vector2.zero);

            if (Input.GetKey(KeyCode.Space))
            {
                if (dashDirection == Vector2.zero)
                {
                    dashDirection = moveInput.normalized;
                }
                Dash();
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
            if (knockBack.GetKnockBack) return;

            if (canDash)
            {
                rb.MovePosition(rb.position + dashDirection * speed * Time.deltaTime);
                return;
            }

            rb.MovePosition(rb.position + moveInput.normalized * speed * Time.deltaTime);
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            canDash = true;
            isDashing = true;
            health.canTakeDamage = false;
            speed += dashSpeed;

            // B?t TrailRenderer trên t?t c? các client
            ToggleTrailRendererServerRpc(true);

            StartCoroutine(EndDashing());
        }
    }

    private IEnumerator EndDashing()
    {
        float dashTime = 0.25f;
        yield return new WaitForSeconds(dashTime);

        canDash = false;
        health.canTakeDamage = true;
        dashDirection = Vector2.zero;
        speed -= dashSpeed;

        // T?t TrailRenderer trên t?t c? các client
        ToggleTrailRendererServerRpc(false);

        float dashCD = 0.7f;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncPlayerPosServerRpc(Vector3 pos)
    {
        otherPos = pos;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleTrailRendererServerRpc(bool isActive)
    {
        ToggleTrailRendererClientRpc(isActive);
    }

    [ClientRpc]
    private void ToggleTrailRendererClientRpc(bool isActive)
    {
        playerTrailRenderer.emitting = isActive;
    }
}
