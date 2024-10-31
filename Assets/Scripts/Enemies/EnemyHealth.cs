using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class EnemyHealth : MonoBehaviour
{
    KnockBack knockback;
    [SerializeField] private float StartingHealth;
    [SerializeField] private float currentHealth;
    private bool isDead=false;
    private GameObject player;
    [SerializeField] private float knockBackThrust = 15f;
    private Animator animator;
    private EnemyAI enemyAI;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject deathVFXPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI=GetComponent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = StartingHealth;
        knockback = GetComponent<KnockBack>();
        animator = GetComponent<Animator>();
        healthBar.maxValue = StartingHealth;
        healthBar.value = currentHealth;
    }

    public void TakedDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        knockback.GettingKnockBack(player.transform, knockBackThrust);
        DetectDeath();
    }
    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            //StartCoroutine(PlayDeathAnimationEnemy());
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    /*private IEnumerator PlayDeathAnimationEnemy()
    {
        // animator.SetTrigger("Death");
        // yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        //Destroy(gameObject);
    }*/
}
