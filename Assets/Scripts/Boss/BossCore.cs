using System;
using Unity.Netcode;
using UnityEngine;

public abstract class BossCore : NetworkBehaviour
{
    public GameObject target;
    public Rigidbody2D body;
    public Animator animator;
    public StateMachine stateMachine;

    public float speed = 500;
    public bool isInAttackRange;
    public bool isAggro;
    public bool isAttacking;

    public BossState state => (BossState)stateMachine.state;

    public void SetupInstances()
    {
        stateMachine = new StateMachine();

        BossState[] childStates = GetComponentsInChildren<BossState>();
        foreach (BossState state in childStates)
        {
            state.Setup(this);
        }

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

    public Vector2 GetDirToTarget()
    {
        return target != null ? (target.transform.position - transform.position).normalized : Vector2.zero;
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
