using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss2 : BossCore
{
    public SpriteRenderer sprite;
    public MissileSpawner missileSpawner;
    public CircleBulletSpawner circleBulletSpawner;
    private EnemyHealth health;
    private float maxHeath;

    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossAttackState attackState;
    public Boss2TransitionState transitionState;

    private bool isInPhaseTwo;
    private bool isInTransition;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<EnemyHealth>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        missileSpawner = GetComponentInChildren<MissileSpawner>();
        circleBulletSpawner = GetComponentInChildren<CircleBulletSpawner>();
    }

    private void Start()
    {
        SetupInstances();
        stateMachine.SetState(idleState);
        maxHeath = health.currentHealth.Value;
    }

    private void Update()
    {
        if (health.isDead.Value) return;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        UpdateSprite();
        


        SetBulletSpawner(stateMachine.GetState != idleState);

        if (state.IsComplete)
        {
            SelectState();
        }
        state.FrameUpdate();
    }

    private void FixedUpdate() 
    {
        state.PhysicsUpdate();
    }

    private void SelectState()
    {
        if (!isInPhaseTwo && health && health.currentHealth.Value <= 25f)
        {
            stateMachine.SetState(transitionState);
            isInPhaseTwo = true;
            return;
        }

        if (!isAggro)
        {
            stateMachine.SetState(idleState);
            return;
        }
    
        if(isInAttackRange && Random.Range(0f, 1f) < 0.5f)
        {
            stateMachine.SetState(attackState);
            return;
        }

        stateMachine.SetState(chaseState); 
    }

        private void UpdateSprite()
    {
        if (isAttacking) return; 
        if (GetDirToTarget().x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }



    private void ShootMissile()
    {
        missileSpawner.Shoot();
    }

    private void SetBulletSpawner(bool _value)
    {
        circleBulletSpawner.isActive = _value;
    }

    private void DoneAttacking()
    {
        if(stateMachine.GetState == attackState)
        {
            stateMachine.state.Exit();
        }
        isInAttackRange = false;
        isAttacking = false;
    }
    
}
