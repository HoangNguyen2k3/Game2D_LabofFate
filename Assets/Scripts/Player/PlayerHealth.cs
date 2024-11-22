using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 10;
    public int currentHealth;
   // public bool isDead=false;
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);
    private Animator animator;
    private Flash flash;
    private KnockBack knockBack;
    [SerializeField] float knockBackThrust = 12f;
    public bool canTakeDamage = true;
    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }
    public void TakedDamageToPlayer(int damage,Transform hitTranform)
    {
        if (!IsOwner) return;
        if(isDead.Value||!canTakeDamage)
        {
            return;
        }
        canTakeDamage = false ;
        StartCoroutine(waitForTakeDamage());
        knockBack.GettingKnockBack(hitTranform, knockBackThrust);
        flash.TriggerFlashServerRpc();
        Debug.Log("hi");
        currentHealth-=damage;
        if (currentHealth <= 0)
        {
            DeathPlayerServerRpc();
            DeathPlayer();
        }
    }
    [ServerRpc]
    public void DeathPlayerServerRpc()
    {
        isDead.Value = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>()&&!isDead.Value)
        {
            TakedDamageToPlayer(1,collision.transform);
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
    private IEnumerator waitForTakeDamage()
    {
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    private void Update()
    {
        
    }
}
