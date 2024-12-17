using System.Collections;
using UnityEngine;

public class Boss3SwordSwing : BossState
{
    private Boss3 boss3;
    private Boss3Arm l2Arm;
    private Boss3Arm r2Arm;

    public AnimationCurve curve;

    private bool firstTimeEnter = true;
    private bool isAttacking;

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>();
        l2Arm = boss3.l2arm;
        r2Arm = boss3.r2arm;
    }

    public override void Enter()
    {
        if (firstTimeEnter)
        {
            firstTimeEnter = false;
            l2Arm.transform.position = transform.position;
            r2Arm.transform.position = transform.position;

            l2Arm.enabled = true;
            r2Arm.enabled = true;

            l2Arm.MoveBackToOrg();
            r2Arm.MoveBackToOrg();
           
        }

        // StartCoroutine()
        l2Arm.transform.right = Vector2.right;
        r2Arm.transform.right = Vector2.right;
        Invoke(nameof(PrepareAttack), 3f);

    }

    public override void PhysicsUpdate()
    {
        if (!isAttacking)
        {
            l2Arm.Idle();
            r2Arm.Idle();
        }
        Vector2 target = boss3.GetTargetPosition() + Vector2.up * 6;
        boss3.movePosition = Vector2.Lerp(boss3.movePosition, target, Time.deltaTime);
    }

    public override void Exit()
    {
        StopAllCoroutines();
        IsComplete = true;
    }

    public void PrepareAttack()
    {
        
        StartCoroutine(AttackSequence());
    }

    public IEnumerator AttackSequence()
    {
        isAttacking = true;

        yield return StartCoroutine(Slash(l2Arm, true));
        StartCoroutine(MoveBack(l2Arm));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Slash(r2Arm));
        
        StartCoroutine(MoveBack(r2Arm));
        yield return new WaitForSeconds(2f);
        StartCoroutine(Slash(l2Arm, true));
        yield return StartCoroutine(Slash(r2Arm));

        StartCoroutine(MoveBack(l2Arm));
        yield return StartCoroutine(MoveBack(r2Arm));        

        isAttacking = false;
        Invoke(nameof(Exit), 1f);
    }

    public IEnumerator Slash(Boss3Arm arm, bool isLeftArm = false)
    {
        
        animator.Play(anim.name);

        Vector2 targetPos = (Vector2)arm.pivot.transform.position + (isLeftArm ? -1 : 1) * Vector2.right + Vector2.up * 2f ;
        yield return StartCoroutine(arm.Move(arm.transform.position,targetPos, anim.length));

        targetPos = (Vector2)boss3.transform.position + Vector2.down * 5;
        Vector2 direction = isLeftArm ? new(-1, 1):  new(-1,-1);
        StartCoroutine(arm.Move(arm.transform.position, targetPos, .3f));

        float _elapsedTime = 0;
        while (_elapsedTime < .3f)
        {
            arm.transform.right = Vector2.Lerp(arm.transform.right, direction, _elapsedTime / 0.3f); 
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        targetPos = (Vector2)arm.transform.position + (isLeftArm ? -1 : 1) * Vector2.right * 3f;
        yield return StartCoroutine(
            arm.Move(arm.transform.position, 
            targetPos, 
            0.2f));
        arm.transform.right = Vector2.right;
    }

    public IEnumerator MoveBack(Boss3Arm arm)
    {

        float _elapsedTime = 0;
        while (_elapsedTime < 0.6f)
        {
            arm.transform.position = Vector2.Lerp(arm.transform.position, arm.pivot.transform.position, Time.deltaTime * 5);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
