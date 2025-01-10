using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 10;
//    public int currentHealth;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(10,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
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
        currentHealth.Value = startingHealth;
        animator = GetComponent<Animator>();
    }
    public void TakedDamageToPlayer(int damage, Transform hitTransform)
    {
        if (isDead.Value || !canTakeDamage)
        {
            return;
        }

        canTakeDamage = false;
        StartCoroutine(waitForTakeDamage());
        knockBack.GettingKnockBack(hitTransform, knockBackThrust);
        knockBack.canBeKnockback = false;
        if(IsOwner)
        flash.TriggerFlashServerRpc();

        TakeDamageServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(int damage)
    {
        if (isDead.Value) return;

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            DeathPlayerServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HealingPlayerHealthServerRpc(int numHealth)
    {
     //   if (!IsServer) return;
        Debug.Log("Add hp");
        if (isDead.Value )
        {
            return;
        }
        if (currentHealth.Value + numHealth > startingHealth)
        {
            currentHealth.Value = startingHealth;
        }
        else
        {
            currentHealth.Value += numHealth;
        }
    }
    [ServerRpc]
    public void DeathPlayerServerRpc()
    {
        DeathPlayer();
        Destroy(gameObject);
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
        /*        Debug.Log("death");animator.speed = 0;
                animator.SetTrigger("isDeath");
                animator.speed = 1;*/
        animator.Play("Death");
    }
    public void DestroyPlayer()
    {
        Debug.Log("destroy player");
        Destroy(gameObject);
    }
    private IEnumerator waitForTakeDamage()
    {
        yield return new WaitForSeconds(0.5f);
        knockBack.canBeKnockback = true;
        canTakeDamage = true;
    }
    private void Update()
    {
        
    }
}
