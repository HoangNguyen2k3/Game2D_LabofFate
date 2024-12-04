using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : BossCore
{

    public SpriteRenderer sprite;
    private EnemyHealth health;
    private float maxHeath;

    public Boss3Arm leftArm;
    public Boss3Arm rightArm;
    public BossState idleState;
    public BossState chaseState;
    public BossState armAttackState;

    public Vector2 movePosition;
    public float angularSpeed = 15f;
    public float circleRad = 0.1f;
    private float currentAngle;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<EnemyHealth>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        movePosition = this.transform.position;
        SetupInstances();
        stateMachine.SetState(armAttackState);
        maxHeath = health.currentHealth.Value;
    }

    private void Update() 
    {
        if (state.IsComplete)
        {
            SelectState();
        }
        state.FrameUpdate();  
    }

    private void FixedUpdate()
    {
        // leftArm.Idle();
        // rightArm.Idle();
        state.PhysicsUpdate();
        MoveInCircle();
    }

    private void SelectState()
    {
        if (!isAggro)
        {
            stateMachine.SetState(idleState);    
        }
        stateMachine.SetState(armAttackState);
    }

    public void MoveInCircle()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
        
        body.MovePosition(movePosition + offset);
    }

    public Vector2 GetTargetPosition()
    {
        return target != null ? target.transform.position : Vector2.zero;
    }
}
