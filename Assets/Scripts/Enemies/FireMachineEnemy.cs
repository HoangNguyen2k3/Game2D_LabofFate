using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FireMachineEnemy : NetworkBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1f;

    private bool isShooting = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!IsServer)
        {
            AttackServerRpc();  
        }
        else
        {
            PerformAttack();  
        }
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        PerformAttack(); 
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        Vector2 targetDirection = FindFirstObjectByType<PlayerController>().transform.position - transform.position;

        for (int i = 0; i < burstCount; i++)
        {
            SpawnBulletClientRpc(targetDirection); 
            yield return new WaitForSeconds(timeBetweenBurst);
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(Vector2 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = direction;
        if (newBullet.TryGetComponent(out Projectile projectile))
        {
            projectile.UpdateMoveSpeed(bulletMoveSpeed);
        }
    }
}
