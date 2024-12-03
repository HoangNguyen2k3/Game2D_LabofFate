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
    private void Start()
    {
        animator = GetComponent<Animator>();
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
        enemyAI = GetComponent<EnemyAI>();
        
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
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
    public void StopAttack()
    {
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
    }
}
