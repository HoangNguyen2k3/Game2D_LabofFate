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
    private SpawnAround spawnAround;
    private Rigidbody2D rb2d;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D_1 = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        spawnAround = GetComponent<SpawnAround>();
        rb2d= GetComponent<Rigidbody2D>();
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
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void SpawnBoom()
    {
        if (!isSmallSemi && IsServer)
        {
            if (enemyHealth.currentHealth.Value <= 35f && !firstTimeSpawn)
            {
                /*  Vector2 temp = transform.position;
                  temp.x -= 5f;
                  GameObject gameObject =  Instantiate(small_SemiBoss, temp, Quaternion.identity);
                  gameObject.GetComponent<NetworkObject>().Spawn();


                  temp.x += 10f;
                  GameObject gameObject2 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                  gameObject2.GetComponent<NetworkObject>().Spawn();
                  temp.x -= 5f;
                  temp.y += 5f;
                  GameObject gameObject3 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                  gameObject3.GetComponent<NetworkObject>().Spawn();
                  firstTimeSpawn = true;*/
                spawnAround.SpawnEnemies(3);
                firstTimeSpawn = true;
            }
            if (enemyHealth.currentHealth.Value <= 15f && !secondTimeSpawn)
            {
                /*Vector2 temp = transform.position;
                temp.x -= 5f;
                GameObject gameObject = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                gameObject.GetComponent<NetworkObject>().Spawn();

                temp.x += 10f;
                GameObject gameObject2 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                gameObject2.GetComponent<NetworkObject>().Spawn();

                temp.x -= 5f;
                temp.y += 5f;
                GameObject gameObject3 = Instantiate(small_SemiBoss, temp, Quaternion.identity);
                gameObject3.GetComponent<NetworkObject>().Spawn();*/
                spawnAround.SpawnEnemies(3);
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
        if (rigidbody2D_1 == null)
        {
            Debug.Log("Loi roi");
            animator = GetComponent<Animator>();
            rigidbody2D_1 = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            enemyHealth = GetComponent<EnemyHealth>();
        }
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeAll;
        col.enabled = false;
    }
    public void FrezzeWaiting()
    {
        if (rigidbody2D_1 == null)
        {
            Debug.Log("Loi roi");
            animator = GetComponent<Animator>();
            rigidbody2D_1 = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            enemyHealth = GetComponent<EnemyHealth>();
        }
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeAll;
      //  col.enabled = false;
    }
    public void UnFrezze()
    {
        if (rigidbody2D_1 == null)
        {
            Debug.Log("Loi roi");
            animator = GetComponent<Animator>();
            rigidbody2D_1 = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            enemyHealth = GetComponent<EnemyHealth>();
        }
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeRotation;
        col.enabled = true;
    }

}
