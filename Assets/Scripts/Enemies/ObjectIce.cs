using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectIce : NetworkBehaviour
{
    [SerializeField] private float damageAttack = 1f;
    [SerializeField] private float timeend = 3f;
    [SerializeField] private GameObject addIceObject;
    bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {

            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakedDamageInIceBullet(damageAttack);
                Vector2 newPos = enemyHealth.transform.position;
                newPos.y -= 1.2f;
                GameObject instance = Instantiate(addIceObject, newPos, Quaternion.identity);
                NetworkObject networkObject1 = instance.GetComponent<NetworkObject>();
                if (networkObject1 != null)
                {
                    networkObject1.Spawn(true);
                }
            }
            //   StartCoroutine(CoolDown());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

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
