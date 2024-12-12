using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ArmPunchState : BossState
{
    private Boss3 boss3;
    private Boss3Arm leftArm;
    private Boss3Arm rightArm;
    public float attackSpeed = 0.5f;

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>();
        leftArm = boss3.leftArm;
        rightArm = boss3.rightArm;
    }

    public override void Enter()
    {

        leftArm.animator.Play("ArmAttackWarn");
        rightArm.animator.Play("ArmAttackWarn");

        float animationTime = leftArm.animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(StartPunch), animationTime + 0.5f);
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    private void StartPunch()
    {
        StartCoroutine(PunchTarget());
    }

    private IEnumerator PunchTarget()
    {
        Vector2 targetPos = boss3.GetTargetPosition();

        leftArm.transform.right = -(targetPos - (Vector2)leftArm.transform.position);
        rightArm.transform.right = targetPos - (Vector2)rightArm.transform.position;

        StartCoroutine(leftArm.Move(leftArm.transform.position, targetPos, attackSpeed));
        yield return StartCoroutine(rightArm.Move(rightArm.transform.position, targetPos, attackSpeed));
        
        leftArm.EnableHitbox();
        rightArm.EnableHitbox();
        yield return new WaitForSeconds(1);
        
        leftArm.DisableHitbox();
        rightArm.DisableHitbox();

        leftArm.MoveBackToOrg();
        rightArm.MoveBackToOrg();
        
        Invoke(nameof(Exit), 2f);
    }
}
