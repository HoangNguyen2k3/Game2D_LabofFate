using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ArmAttackState : BossState
{
    public Boss3 boss3;
    public Boss3Arm leftArm;
    public Boss3Arm rightArm;

    private void Awake()
    {
        leftArm = boss3.leftArm;
        rightArm = boss3.rightArm;
    }

    public override void Enter()
    {
        leftArm.isDonePunching = false;
        rightArm.isDonePunching = false;
        leftArm.animator.Play("ArmAttackWarn");
        rightArm.animator.Play("ArmAttackWarn");
    }

    public override void FrameUpdate()
    {
        if (leftArm.isDonePunching && rightArm.isDonePunching) Exit();
    }

    public override void PhysicsUpdate()
    {

    }

    public override void Exit()
    {
        IsComplete = true;
    }
}
