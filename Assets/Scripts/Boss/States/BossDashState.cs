using UnityEngine;

public class BossDashState : BossState
{
    [field: SerializeField] public float DashDuration {get; private set;} = 0.3f;
    public Vector2 dashDirection;
    public float delayDash = 0.5f;
    public LineRenderer lineRenderer;

    public override void Enter() 
    {   
        dashDirection = boss.GetDirToTarget();
        ShowWarnLine();
        animator.Play(anim.name); 
    }

    public override void FrameUpdate() 
    {
        if (ElapsedTime > delayDash) {
            lineRenderer.enabled = false;
        }
    }
    public override void PhysicsUpdate() 
    {
        if (ElapsedTime > delayDash)
        {
            Dash();
        } 
        else 
        {
            boss.ApplyStopFriction();
        }
    }

    public override void Exit() 
    {
        IsComplete = true;
    }

    private void Dash()
    {   
        if (ElapsedTime < DashDuration + delayDash)
        {
            body.velocity = Time.deltaTime * boss.speed * 4 * dashDirection;
        }
        else 
        {
            boss.ApplyStopFriction(0.85f);
            if (body.velocity == Vector2.zero) Exit();
        }
    }

    private void ShowWarnLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, body.position);
        lineRenderer.SetPosition(1, (60 * dashDirection) + body.position);
    }
}
