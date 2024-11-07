using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (dashDirection == Vector2.zero)
            {
                dashDirection = moveInput.normalized;
            }
            Dash();
        }
    }
    private void FixedUpdate()
    {
        if (canDash)
        {
            rb.MovePosition(rb.position + dashDirection * speed * Time.deltaTime);
            return;
        }
        rb.MovePosition(rb.position +moveInput.normalized*speed*Time.deltaTime);
    }
    private void Dash()
    {
        if (!isDashing)
        {
            canDash = true;
            isDashing = true;
            speed += dashSpeed;
            playerTrailRenderer.emitting = true;
            StartCoroutine(EndDashing());
        }
    }
    private IEnumerator EndDashing()
    {
        float dashTime = 0.25f;
        yield return new WaitForSeconds(dashTime);
        canDash = false;
        dashDirection=Vector2.zero;
        speed -= dashSpeed;
        playerTrailRenderer.emitting = false;
        float dashCD = 0.7f;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}