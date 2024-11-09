using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BossChaseState : BossState
{
    [field: SerializeField] public float ReturnTime {get; private set;} = 3f; // Exit state after {returnTime} second

    public override void Enter() 
    { 
        animator.Play(anim.name);
    }

    public override void FrameUpdate() 
    {

        if (ElapsedTime > ReturnTime) Exit();
    }

    public override void PhysicsUpdate() 
    {
        if (!boss.isInAttackRange)
        {
            body.velocity = boss.speed * Time.deltaTime * boss.GetDirToTarget();
            return;
        } 

        Exit();

        if (!boss.isAggro)
        {
            Exit();
        }

        boss.ApplyStopFriction();

    }

    public override void Exit() 
    {
        IsComplete = true;
    }
}
