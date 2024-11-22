using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossState
{
    // [field: SerializeField] public float TimeToComplete {get; private set;} = 0.7f;


    public override void Enter() 
    {
        animator.Play(anim.name);
    }

    public override void FrameUpdate() 
    {
        if (!boss.isInAttackRange)
        {
            Exit();
        }
    }

    public override void PhysicsUpdate() 
    {
        boss.ApplyStopFriction();

    }
    
    public override void Exit() 
    {
        if (!boss.isAttacking)
        {
            IsComplete = true;
        }
    }
}
