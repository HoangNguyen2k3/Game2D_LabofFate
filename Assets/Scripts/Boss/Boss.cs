using Unity.Netcode;
using UnityEngine;

public class Boss : BossCore
{
    public SpriteRenderer sprite;
    public GameObject attackSprite;
    public Animator attackSpriteLowerAnim;
    public Animator attackSpriteUpperAnim;

    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossAttackState attackState;
    public BossDashState dashState;

    public enum Phase {PHASE1, PHASE2}
    private Phase currentPhase;
    private EnemyHealth health;

    public SpriteRenderer attackUpperBody;
    public SpriteRenderer attackLowerBody;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        SetupInstances();
        stateMachine.SetState(idleState);
        SetPhase(Phase.PHASE1);
    }

    private void Update()
    {
        if (health.isDead.Value) return;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        UpdateSprite();

        if (health.currentHealth.Value <= 15f)
        {
            SetPhase(Phase.PHASE2);
        }
        if (state.IsComplete)
        {
            SelectState();
        }
        state.FrameUpdate();
    }

    private void FixedUpdate() 
    {
        // Debug.Log(body.velocity);
        state.PhysicsUpdate();
    }

    private void SelectState()
    {
        switch (currentPhase)
        {
            case Phase.PHASE1:
                PhaseOneMoveset();
                break;
            case Phase.PHASE2:
                PhaseTwoMoveset();
                break;
        }
    }

    private void PhaseOneMoveset()
    {
        // Debug.Log("Using phase 1 moveset");
        if (!isAggro)
        {
            stateMachine.SetState(idleState);
            return;
        }
        if (!isInAttackRange)
        {
            stateMachine.SetState(chaseState, true);
            return;
        }
        stateMachine.SetState(attackState);
    }

    private void PhaseTwoMoveset()
    {
        // Debug.Log("Using phase 2 moveset");
        if (!isAggro)
        {
            stateMachine.SetState(idleState);
            return;
        }
        if (!isInAttackRange)
        {
            stateMachine.SetState(Random.Range(1, 100) <= 70 ? chaseState : dashState, true);
            return;
        }
        stateMachine.SetState(attackState);
    }

    private void UpdateSprite()
    {
        attackUpperBody.material = sprite.material;
        attackLowerBody.material = sprite.material;
        if (isAttacking) return; 
        if (GetDirToTarget().x < 0)
        {
            sprite.flipX = true;
            attackSprite.transform.localScale = new Vector3(-1f, 1f);
        }
        else
        {
            sprite.flipX = false;
            attackSprite.transform.localScale = new Vector3(1f, 1f);
        }

        if (attackSpriteLowerAnim.isActiveAndEnabled == true)
        {
            if (body.velocity != Vector2.zero)
            {
                attackSpriteLowerAnim.Play("LowerBodyChasing");
            }
            else
            {
                attackSpriteLowerAnim.Play("LowerBodyStanding");
            }
        }
    }

    public void SetPhase(Phase phase)
    {
        if (currentPhase == Phase.PHASE1 && phase == Phase.PHASE2)
        {
            stateMachine.SetState(dashState, true);
        }
        currentPhase = phase;

    }
}
