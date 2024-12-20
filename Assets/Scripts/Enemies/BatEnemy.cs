using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BatEnemy : NetworkBehaviour,IEnemy
{
    private Animator animator;
    [SerializeField] private GameObject attackLeft;
    [SerializeField] private GameObject attackRight;
    private EnemyAI enemyAI;
    [SerializeField] private GameObject bulletBat;
    private void Start()
    {
        animator = GetComponent<Animator>();
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
        enemyAI = GetComponent<EnemyAI>();
        
    }
    public void Attack()
    {
        int temp = Random.Range(1, 3);
        if (temp == 2)
        {
            animator.SetTrigger("Attack");
        }
        else if(temp==1)
        {
            animator.SetTrigger("Attack2");
        }
       
    }
    public void StartAttack()
    {
        if (enemyAI.target)
        {
            if (enemyAI.target.transform.position.x < transform.localPosition.x)
            {
                attackRight.SetActive(false);
                attackLeft.SetActive(true);
            }
            else
            {
                attackLeft.SetActive(false);
                attackRight.SetActive(true);
            }
        }
    }
    public void StartAttack2()
    {
        
        if (enemyAI.target)
        {
            Instantiate(bulletBat, transform.position, Quaternion.identity);
            if (enemyAI.target.transform.position.x < transform.localPosition.x)
            {
                attackRight.SetActive(false);
                attackLeft.SetActive(true);
            }
            else
            {
                attackLeft.SetActive(false);
                attackRight.SetActive(true);
            }
        }
    }
    public void StopAttack()
    {
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
    }
}
