using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ChaseState : BossState
{
    [field: SerializeField] public float ReturnTime {get; private set;} = 3f; // Exit state after {returnTime} second

    public Boss3 boss3;
    public override void Enter() 
    { 
        animator.Play(anim.name);
    }

    public override void FrameUpdate() 
    {
        boss3.leftArm.Idle();
        boss3.rightArm.Idle();
        if (ElapsedTime > ReturnTime) Exit();
    }

    public override void PhysicsUpdate() 
    {
        Vector2 target = new(boss3.GetTargetPosition().x, boss3.GetTargetPosition().y + 5);
        boss3.movePosition = Vector2.Lerp(boss3.movePosition, target, Time.deltaTime * 2);

        if (!boss.isAggro)
        {
            Exit();
        }
    }

    public override void Exit() 
    {
        IsComplete = true;
    }

}
