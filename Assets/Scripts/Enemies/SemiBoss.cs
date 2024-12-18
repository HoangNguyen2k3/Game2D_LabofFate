using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiBoss : MonoBehaviour,IEnemy
{
    private Animator animator;
   [SerializeField] private GameObject Bullet;
    private Rigidbody2D rigidbody2D_1;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D_1 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void SpawnBoom()
    {
      Instantiate(Bullet, transform.position, Quaternion.identity);
    }
    public void Frezze()
    {
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void UnFrezze()
    {
        rigidbody2D_1.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
