using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ChaseState : BossState
{
    [field: SerializeField] public float ReturnTime {get; private set;} = 3f; // Exit state after {returnTime} second

    private Boss3 boss3;
    private Boss3Arm leftArm;
    private Boss3Arm rightArm;

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>(); 
        leftArm = boss3.leftArm;
        rightArm = boss3.rightArm;
    }

    public override void Enter() 
    { 
    }

    public override void FrameUpdate() 
    {
        leftArm.Idle();
        rightArm.Idle();
        if (ElapsedTime > ReturnTime) Exit();
    }

    public override void PhysicsUpdate() 
    {
        Vector2 target = boss3.GetTargetPosition() + Vector2.up * 4;
        boss3.movePosition = Vector2.Lerp(boss3.movePosition, target, Time.deltaTime * 2);
    }

    public override void Exit() 
    {
        IsComplete = true;
    }

}
