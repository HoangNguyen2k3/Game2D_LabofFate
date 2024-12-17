using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ArmSmashState : BossState
{
    private Boss3 boss3;
    private Boss3Arm leftArm;
    private Boss3Arm rightArm;
    public float attackSpeed = 0.3f;

    public GameObject explosion;

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>();
        leftArm = boss3.leftArm;
        rightArm = boss3.rightArm;
    }

    public override void Enter()
    {
        StartCoroutine(SmashTarget());
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    public IEnumerator SmashTarget()
    {
        yield return StartCoroutine(SmashArm(leftArm));
        yield return StartCoroutine(SmashArm(rightArm));

        Invoke(nameof(Exit), 1f);

    }

    private IEnumerator SmashArm(Boss3Arm arm)
    {
        arm.EnableHitbox();
        Vector2 targetPos = boss3.GetTargetPosition() + Vector2.up * 4;
        yield return StartCoroutine(arm.Move(arm.transform.position, targetPos, attackSpeed));
        // yield return StartCoroutine(rightArm.Move(rightArm.transform.position, targetPos, attackSpeed));

        arm.animator.Play("ArmAttackWarn");
        float animationTime = arm.animator.GetCurrentAnimatorStateInfo(0).length + 0.5f;

        float _elapsedTime = 0;
        while (_elapsedTime < animationTime)
        {
            targetPos = boss3.GetTargetPosition() + Vector2.up * 4;

            arm.transform.position = Vector2.Lerp(
                arm.transform.position, 
                targetPos, 
                Time.deltaTime * 5);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(arm.Move(
            arm.transform.position, 
            (Vector2)arm.transform.position + Vector2.down * 4, 
            attackSpeed/2));
    

        if (explosion) Instantiate(explosion, arm.transform.position + Vector3.down, Quaternion.identity);
        arm.DisableHitbox();
        arm.MoveBackToOrg();
    }
}
