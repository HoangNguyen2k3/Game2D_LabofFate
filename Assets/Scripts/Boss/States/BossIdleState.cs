using System.Runtime.CompilerServices;
using UnityEngine;

public class BossIdleState : BossState
{

    public override void Enter() { 
        animator.Play(anim.name);
    }

    public override void FrameUpdate() {
        if (boss.isAggro || boss.isInAttackRange)
        {
            Exit();   
        }
    }

    public override void PhysicsUpdate() {
        boss.ApplyStopFriction();
    }

    public override void Exit() {
        IsComplete = true;
    }
}
