using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TakedDamageToEnemies : NetworkBehaviour
{
    [SerializeField] private float damageToEnemy = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return; 

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamageServerRpc(damageToEnemy);
        }
    }
}
