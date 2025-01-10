using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public class FireMachineEnemy : NetworkBehaviour, IEnemy
{
 // [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1f;
    
    private EnemyStandAI enemyStandAI;
    private bool isShooting = false;
    private Animator animator;
    private Transform target;

    private ObjectPoolingManager poolingManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyStandAI = GetComponent<EnemyStandAI>();
        poolingManager = GetComponent<ObjectPoolingManager>();
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

        if (enemyStandAI.target)
        {
            target = enemyStandAI.target;
        }
        else
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                isShooting = false;
                yield break;
            }
        }

        Vector2 targetDirection = target.position - transform.position;

        for (int i = 0; i < burstCount; i++)
        {
            SpawnBulletServerRpc(targetDirection);
            yield return new WaitForSeconds(timeBetweenBurst);
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector2 direction)
    {
        GameObject newBullet = poolingManager.GetBullet();
        newBullet.transform.position = transform.position;
        newBullet.transform.right = direction;

        if (newBullet.TryGetComponent(out Projectile projectile))
        {
            projectile.UpdateMoveSpeed(bulletMoveSpeed);
        }

        SpawnBulletClientRpc(newBullet.GetComponent<NetworkObject>().NetworkObjectId, direction);
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(ulong bulletId, Vector2 direction)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(bulletId, out var netObj))
            return;

        GameObject newBullet = netObj.gameObject;
        newBullet.transform.position = transform.position;
        newBullet.transform.right = direction;
    }

}
