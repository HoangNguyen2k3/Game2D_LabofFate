using System.Collections;
using System.Collections.Generic;
using Mono.CSharp.yyParser;
using UnityEngine;

public class Boss3ArmLaserAttackState : BossState
{
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
        Invoke(nameof(StartAttack), 0.1f);
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    private void StartAttack()
    {
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        // Set arm position
        Vector2 targetPos = boss3.GetTargetPosition();
        Vector2[] leftArmPossiblePos = {Vector2.left * 4, Vector2.right * 4}; 
        Vector2[] rightArmPossiblePos = {Vector2.down * 4, Vector2.up * 4}; 

        int leftChoice = Random.Range(0, 2);
        int rightChoice = Random.Range(0, 2);

        Vector2 position1 = targetPos + leftArmPossiblePos[leftChoice];
        Vector2 position2 = targetPos + rightArmPossiblePos[rightChoice];

        leftArm.transform.right = leftArmPossiblePos[leftChoice];
        rightArm.transform.right = -rightArmPossiblePos[rightChoice];

        // Both hand move to setposition
        StartCoroutine(leftArm.Move(leftArm.transform.position, position1, 0.5f));
        yield return StartCoroutine(rightArm.Move(rightArm.transform.position, position2, 0.5f));

        // Play warning animation and chase target
        leftArm.animator.Play("ArmAttackWarn");
        rightArm.animator.Play("ArmAttackWarn");

        float animationTime = leftArm.animator.GetCurrentAnimatorStateInfo(0).length + 0.5f;
        
        float _elapsedTime = 0;
        while (_elapsedTime < animationTime)
        {
            targetPos = boss3.GetTargetPosition();
            position1 = targetPos + leftArmPossiblePos[leftChoice];
            position2 = targetPos + rightArmPossiblePos[rightChoice];

            leftArm.transform.position = Vector2.Lerp(
                leftArm.transform.position, 
                position1, 
                Time.deltaTime * 5);

            rightArm.transform.position = Vector2.Lerp(
                rightArm.transform.position, 
                position2, 
                Time.deltaTime * 5);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftArm.EnableHitbox();
        rightArm.EnableHitbox();

        // Shoot laser
        leftArm.ShootLaser();
        rightArm.ShootLaser();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(leftArm.Move(
            leftArm.transform.position, 
            (Vector2)leftArm.transform.position + rightArmPossiblePos[1-rightChoice] * 2, 
            2f
            )
        );

        yield return StartCoroutine(rightArm.Move(
            rightArm.transform.position, 
            (Vector2)rightArm.transform.position + leftArmPossiblePos[1-leftChoice] * 2, 
            2f
            )
        );

        leftArm.EnableHitbox();
        rightArm.EnableHitbox();

        leftArm.StopShootLaser();
        rightArm.StopShootLaser();

        yield return new WaitForSeconds(1f);
        // Back to original position
        leftArm.MoveBackToOrg();
        rightArm.MoveBackToOrg();

        Invoke(nameof(Exit), 2f);
    }
}
