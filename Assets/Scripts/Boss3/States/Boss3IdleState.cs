using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3IdleState : BossState
{
    private Boss3 boss3;

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>(); 
    }

    public override void Enter() { 

    }

    public override void FrameUpdate() {
        boss3.leftArm.Idle();
        boss3.rightArm.Idle();
        if (boss.isAggro)
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
