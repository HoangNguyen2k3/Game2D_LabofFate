using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 10;
    private int maxHeatlh;
    public int currentHealth;
    public bool isDead=false;
    private Animator animator;

    private void Awake()
    {
        maxHeatlh = startingHealth;
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }
    public void TakedDamageToPlayer(int damage)
    {
        currentHealth-=damage;
        if (currentHealth <= 0)
        {
            isDead = true;
            DeathPlayer();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            TakedDamageToPlayer(1);
        }
    }
    private void DeathPlayer()
    {
        animator.SetTrigger("isDeath");
    }
    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        
    }
}
