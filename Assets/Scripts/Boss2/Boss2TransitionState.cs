using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2TransitionState : BossState
{

    public MissileSpawner missileSpawner;
    public CircleBulletSpawner circleBulletSpawner;

    public override void Enter() 
    {
        animator.Play(anim.name);
        circleBulletSpawner.isActive = false;
        StartCoroutine(StartPhaseTwoTransition());
    }

    public override void FrameUpdate() 
    {
        //
    }

    public override void PhysicsUpdate() 
    {
        boss.ApplyStopFriction();
    }
    
    private IEnumerator StartPhaseTwoTransition()
    {
        yield return new WaitForSeconds(5f);
        
        circleBulletSpawner.isActive = true;
        circleBulletSpawner.numberOfProjectile = 3;
        circleBulletSpawner.rotateDeg = 1;
        circleBulletSpawner.shootCooldown = 0.05f;
        circleBulletSpawner.numberOfBurst = 100;
        
        yield return new WaitForSeconds(5f);
        circleBulletSpawner.isActive = false;

        circleBulletSpawner.numberOfProjectile = 6;
        circleBulletSpawner.rotateDeg = 10;
        circleBulletSpawner.shootCooldown = 0.2f;
        circleBulletSpawner.numberOfBurst = 10;
        


        yield return new WaitForSeconds(2f);

        circleBulletSpawner.isActive = true;
        Exit();

    }

    public override void Exit() 
    {
        if (!boss.isAttacking)
        {
            IsComplete = true;
        }
    }
}
