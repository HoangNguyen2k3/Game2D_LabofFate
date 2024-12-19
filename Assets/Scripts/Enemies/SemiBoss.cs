using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SemiBoss : NetworkBehaviour,IEnemy
{
    private Animator animator;
   [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject Bullet2;
    private Rigidbody2D rigidbody2D_1;
    private Collider2D col;
    private EnemyHealth enemyHealth;
    [SerializeField] private GameObject small_SemiBoss;
    bool firstTimeSpawn = false;
    bool secondTimeSpawn = false;
    [SerializeField] bool isSmallSemi = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D_1 = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
    void Update()
    {
    }
    public void Attack()
    {
        StartCoroutine(AttackMulti());
    }
    private IEnumerator AttackMulti()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
    }
    public void SpawnBoom()
    {
        if (!isSmallSemi)
        {
            if (enemyHealth.currentHealth.Value == 35f && !firstTimeSpawn)
            {
                Vector2 temp = transform.position;
                temp.x -= 5f;
                GameObject gameObject =  Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject.GetComponent<NetworkObject>().Spawn();
                }
                
                temp.x += 10f;
                GameObject gameObject2 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject2.GetComponent<NetworkObject>().Spawn();
                }
                temp.x -= 5f;
                temp.y += 5f;
                GameObject gameObject3 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject3.GetComponent<NetworkObject>().Spawn();
                }
                firstTimeSpawn = true;
            }
            if (enemyHealth.currentHealth.Value == 15f && !secondTimeSpawn)
            {
                Vector2 temp = transform.position;
                temp.x -= 5f;
                GameObject gameObject = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject.GetComponent<NetworkObject>().Spawn();
                }

                temp.x += 10f;
                GameObject gameObject2 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject2.GetComponent<NetworkObject>().Spawn();
                }
                temp.x -= 5f;
                temp.y += 5f;
                GameObject gameObject3 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                if (IsServer)
                {
                    gameObject3.GetComponent<NetworkObject>().Spawn();
                }
                secondTimeSpawn = true;
            }
        }

        if (enemyHealth.currentHealth.Value * 2f >= enemyHealth.StartingHealth)
        {
            Instantiate(Bullet, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(Bullet2, transform.position, Quaternion.identity);
        }
      
    }
    public void Frezze()
    {
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeAll;
        col.enabled = false;
    }
    public void UnFrezze()
    {
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeRotation;
        col.enabled = true;
    }
}
