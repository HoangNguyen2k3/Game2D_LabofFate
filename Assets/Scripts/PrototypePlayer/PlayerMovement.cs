using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] TrailRenderer tr;

    [Header("Movement settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Dash settings")]
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0.2f;

    bool isDashing = false;
    bool canDash = true;

    public Health hp;

    Vector2 direction;

    void Update()
    {
        if(isDashing) return;

        direction.x = Input.GetAxisRaw("Horizontal");    
        direction.y = Input.GetAxisRaw("Vertical");
        direction.Normalize();    

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    void  FixedUpdate() {
        if (isDashing) return;
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed );
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 dashDir = direction;

        rb.velocity = new Vector2(dashDir.x * dashSpeed, dashDir.y * dashSpeed );
        tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;  
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
