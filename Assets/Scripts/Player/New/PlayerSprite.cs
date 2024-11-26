using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSprite : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SlashManagerCombo slashManagerCombo;
    [SerializeField] private GameObject SlashHitboxes;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth health;

    private Vector2 MoveInput => playerController.GetMoveInput();
    private string DirectionStr => playerController.DirectionStr;
    private bool IsDashing => playerController.isDashing;
    private bool IsAttacking => slashManagerCombo.isAttacking.Value;

    private bool isDead = false;

    // NetworkVariable for flipX
    private NetworkVariable<bool> networkFlipX = new NetworkVariable<bool>(false);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = gameObject.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        isDead = health.isDead.Value;
        if (IsOwner)
        {
            UpdateSprite();
        }

        // Apply the flipX state from the NetworkVariable to the SpriteRenderer
        spriteRenderer.flipX = networkFlipX.Value;
        if (spriteRenderer.flipX)
        {
            SlashHitboxes.transform.localScale = new Vector3(-1, 1);
        }
        else
        {
            SlashHitboxes.transform.localScale = new Vector3(1, 1);
        }
    }

    private void UpdateSprite()
    {
        HandleSpriteFlip();
        SetAnimation();
    }

    private void HandleSpriteFlip()
    {
        if (MoveInput.x < 0 && !spriteRenderer.flipX)
        {
            SetSpriteServerRpc(true);  // Flip sprite
        }
        else if (MoveInput.x > 0 && spriteRenderer.flipX)
        {
            SetSpriteServerRpc(false);  // Unflip sprite
        }
    }

    [ServerRpc]
    private void SetSpriteServerRpc(bool state)
    {
        // Update the NetworkVariable on the server
        networkFlipX.Value = state;
    }

    private void SetAnimation()
    {
        if (IsAttacking || isDead) return;
        if (MoveInput != Vector2.zero && DirectionStr != null)
        {
            if (IsDashing)
            {
                animator.Play("Dash" + DirectionStr);
                return;
            }
            animator.Play("Run" + DirectionStr);
        }
        else
        {
            animator.Play("Idle" + DirectionStr);
        }
    }
}
