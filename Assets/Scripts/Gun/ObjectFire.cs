using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectFire : NetworkBehaviour
{
    [SerializeField] private float damageAttack;
    [SerializeField] private float timeend = 3f;
    bool canDamage = true;
    public bool isIce = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage&&!isIce&&IsServer)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakedDamageNotInPlayer(damageAttack, transform);
            }
            StartCoroutine(CoolDown());
        }
        if (canDamage && isIce && IsServer)
        {

/*            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakedDamageNotInPlayer(damageAttack, transform);

            }
            StartCoroutine(CoolDown());*/
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isIce) { return; }
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth)
        {
            enemyHealth.TakedDamageInIceBullet(damageAttack);
            enemyHealth.IceBullet(damageAttack);
        }
    }
    private void Start()
    {
        if (IsServer)
        {
   StartCoroutine(DestroyGameOjectCurrent());
        }
     
    }
    private IEnumerator DestroyGameOjectCurrent()
    {
        yield return new WaitForSeconds(timeend);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
    private IEnumerator CoolDown()
    {
        canDamage = false;
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
        
    }
}
