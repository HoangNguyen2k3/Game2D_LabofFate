using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.8f;

    private Vector2 direction;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        tr.time = 0.3f;
    }

    private void Update()
    {
        if (isDashing) return; // No input while dashing
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");    
        direction.Normalize();

        if (Input.GetKeyDown(KeyCode.Space) && canDash) 
        {
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 dashDir = direction;

        rb.velocity = new Vector2(dashDir.x * dashSpeed, dashDir.y * dashSpeed);
        tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;  
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
