using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SlashManagerCombo slashManagerCombo;
    [SerializeField] private GameObject SlashHitboxes;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 MoveInput => playerController.GetMoveInput();
    private string DirectionStr => playerController.DirectionStr;
    private bool IsDashing => playerController.isDashing;
    private bool IsAttacking => slashManagerCombo.isAttacking.Value;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        HandleSpriteFlip();
        SetAnimation();
    }

    private void HandleSpriteFlip()
    {
        if (!spriteRenderer.flipX && MoveInput.x < 0)
        {
            spriteRenderer.flipX = true;
            SlashHitboxes.transform.localScale = new Vector3(-1,1);
        }
        else if (spriteRenderer.flipX && MoveInput.x > 0)
        {
            SlashHitboxes.transform.localScale = new Vector3(1,1);
            spriteRenderer.flipX = false;
        }
    }

    private void SetAnimation()
    {
        if (IsAttacking) return;
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
