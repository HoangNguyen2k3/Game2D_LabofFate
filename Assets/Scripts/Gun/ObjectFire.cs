using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFire : MonoBehaviour
{
    [SerializeField] private float damageAttack;
    [SerializeField] private float timeend = 3f;
    bool canDamage = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakedDamageNotInPlayer(damageAttack, transform);
            }
       //    StartCoroutine(CoolDown());
        }
    }
    private void Start()
    {
        StartCoroutine(DestroyGameOjectCurrent());
    }
    private IEnumerator DestroyGameOjectCurrent()
    {
        yield return new WaitForSeconds(timeend);
        Destroy(gameObject);
    }
    private IEnumerator CoolDown()
    {
        canDamage = false;
        yield return new WaitForSeconds(1f);
        canDamage = true;
        
    }
}
