using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedDamageToEnemies : MonoBehaviour
{
    [SerializeField] private float damageToEnemy = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakedDamage(damageToEnemy);
        }
    }
}
