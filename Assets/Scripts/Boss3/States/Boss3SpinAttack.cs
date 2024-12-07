using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class Boss3SpinAttack : BossState
{
    private Boss3 boss3;
    private Boss3Arm leftArm;
    private Boss3Arm rightArm;
    public CircleBulletSpawner bulletSpawner;
    private bool canChase;

    public AnimationCurve curve;

    public float spinTime = 15f;
    public float armDistance = 3f;
    public float speed = 2f;
    public float angularSpeed = 10f;
    private float currentAngle = 1.5708f; // ~ 90deg in rad

    private void Awake()
    {
        boss3 = GetComponentInParent<Boss3>();
        bulletSpawner = boss3.GetComponentInChildren<CircleBulletSpawner>();

        leftArm = boss3.leftArm;
        rightArm = boss3.rightArm;
    }

    public override void Enter()
    {
        animator.Play(anim.name);

        StartCoroutine(PrepareSpin());

    }

    public override void PhysicsUpdate()
    {
        if (canChase)
        {
            ChaseAndSpin();
        }

    }

    public override void Exit()
    {
        IsComplete = true;
    }

    private IEnumerator PrepareSpin()
    {
        yield return StartCoroutine(MoveArmToPos(anim.length + 0.5f));
        bulletSpawner.isActive = true;
        canChase = true;

        yield return new WaitForSeconds(spinTime);
        canChase = false;
        bulletSpawner.isActive = false;

        StartCoroutine(MoveArmToPos(2f));
        yield return StartCoroutine(StopMoving());
        leftArm.MoveBackToOrg();
        rightArm.MoveBackToOrg();
        Invoke(nameof(Exit), 1f);
    }

    private IEnumerator MoveArmToPos(float _time)
    {
        // leftArm.transform.right = -Vector2.left;
        // rightArm.transform.right = Vector2.right;

        leftArm.OpenFist();
        rightArm.OpenFist();

        float elapsedTime = 0;
        while (elapsedTime < _time)
        {
            leftArm.transform.position = Vector2.Lerp(
                leftArm.transform.position, 
                (Vector2)transform.position + Vector2.left * armDistance, 
                elapsedTime/_time
            );

            rightArm.transform.position = Vector2.Lerp(
                rightArm.transform.position, 
                (Vector2)transform.position + Vector2.right * armDistance, 
                elapsedTime/_time
            );
            
            leftArm.transform.right = Vector2.Lerp(
                leftArm.transform.right,
                Vector2.down,
                elapsedTime/_time
            );
            
            rightArm.transform.right = Vector2.Lerp(
                rightArm.transform.right,
                Vector2.down,
                elapsedTime/_time
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void ChaseAndSpin()
    {
        
        boss3.movePosition += speed * Time.deltaTime * boss3.GetDirToTarget();
        
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * armDistance;
        currentAngle += angularSpeed * Time.deltaTime;
        
        leftArm.transform.position = (Vector2)transform.position - offset;
        rightArm.transform.position = (Vector2)transform.position + offset;

        leftArm.transform.right = Vector3.Cross(offset, Vector3.forward);
        rightArm.transform.right = Vector3.Cross(offset, Vector3.forward);
    }

    public IEnumerator StopMoving()
    { 
        float currentSpeed = speed;
        while (currentSpeed > 0)
        {
            boss3.movePosition += currentSpeed * Time.deltaTime * boss3.GetDirToTarget();
            currentSpeed -= Time.deltaTime * 3;
            yield return null;
        }
    }
}
