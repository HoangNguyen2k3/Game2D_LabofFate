using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 dashDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    private bool isDashing = false;
    private bool canDash = false;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
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
        }
    

    private void FixedUpdate()
    {


            if (canDash)
            {
                rb.MovePosition(rb.position + dashDirection * speed * Time.deltaTime);
                return;
            }

            rb.MovePosition(rb.position + moveInput.normalized * speed * Time.deltaTime);
    }

    private void Dash()
    {
        if (!isDashing)
        {
            canDash = true;
            isDashing = true;
            speed += dashSpeed;

            StartCoroutine(EndDashing());
        }
    }

    private IEnumerator EndDashing()
    {
        float dashTime = 0.25f;
        yield return new WaitForSeconds(dashTime);

        canDash = false;
        dashDirection = Vector2.zero;
        speed -= dashSpeed;
        float dashCD = 0.7f;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
