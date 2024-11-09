using UnityEngine;

public class BossAttackState : BossState
{
    [field: SerializeField] public float TimeToComplete {get; private set;} = 0.7f;

    public override void Enter() 
    {
        animator.Play(anim.name);
    }

    public override void FrameUpdate() 
    {
        if (!boss.isInAttackRange) 
            // Exit();
            StartCoroutine(DelayedExit(TimeToComplete));
    }

    public override void PhysicsUpdate() 
    {
        // body.velocity = Vector2.zero;
        boss.ApplyStopFriction();

    }
    
    public override void Exit() 
    {
        IsComplete = true;            
    }


}
