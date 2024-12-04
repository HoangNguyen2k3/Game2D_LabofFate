using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3IdleState : BossState
{
    public Boss3 boss3;

    public override void Enter() { 
        animator.Play(anim.name);
    }

    public override void FrameUpdate() {
        boss3.leftArm.Idle();
        boss3.rightArm.Idle();
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
