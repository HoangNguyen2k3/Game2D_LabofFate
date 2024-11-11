using Unity.Netcode;
using UnityEngine;

public class Boss : BossCore
{

    public SpriteRenderer sprite;
    public GameObject attackSprite;
    public Animator attackSpriteLowerAnim;
    public Animator attackSpriteUpperAnim;

    [field: SerializeField] public float speed {get; private set;} = 500;
    [field: SerializeField] public bool isInAttackRange {get; private set;} = false;
    [field: SerializeField] public bool isAggro {get; private set;} = false;

    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossAttackState attackState;
    public BossDashState dashState;

    public enum Phase {PHASE1, PHASE2}
    private Phase currentPhase;

    private EnemyHealth health;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        boss = this;
    }

    private void Start()
    {
        SetupInstances();
        stateMachine.SetState(idleState);
        SetPhase(Phase.PHASE1);
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        UpdateSprite();

        if (health.currentHealth.Value <= 50f)
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

    public Vector2 GetDirToTarget()
    {
        return target != null ? (target.transform.position - transform.position).normalized : Vector2.zero;
    }

    public void ApplyStopFriction(float slowRate = 0.9f)
    {
        slowRate = Mathf.Clamp(slowRate, 0f, 1f);
        if (Mathf.Abs(body.velocity.x) > 0.1f 
        || Mathf.Abs(body.velocity.y) > 0.1f) // Check if velocity is close to zero
        {
            body.velocity = slowRate * 50 * Time.deltaTime * body.velocity;
        }
        else 
        {
            body.velocity = Vector2.zero;
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

    public void SetAttackRangeStatus(bool _isInAttackRange)
    {
        isInAttackRange = _isInAttackRange;
    }

    public void SetAggroRangeCheck(bool _isAggro)
    {
        isAggro = _isAggro;
    }

}
