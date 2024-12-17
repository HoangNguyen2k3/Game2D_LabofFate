using System.Collections;
using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class EnemyHealth : NetworkBehaviour
{
    KnockBack knockback;
    [SerializeField] private float StartingHealth;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private GameObject player;
    [SerializeField] private float knockBackThrust = 15f;
    private Animator animator;
    private EnemyAI enemyAI;
    private EnemyStandAI enemyStand;
    private EnemyPathFinding enemyPathFinding;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject deathVFXPrefab;
  //  [SerializeField] private TextMeshProUGUI numCurrentHealth;
  //  [SerializeField] private TextMeshProUGUI name_enemy;
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);
    private Flash flash;
    private Collider2D collider_enemy;

    public bool isInteractive = true;

    [SerializeField] private float addTimeAnim=1f;

    [SerializeField] private GameObject dropItems_small;
    [SerializeField] private GameObject dropItems_medium;
    [SerializeField] private GameObject dropItems_big;

    void Start()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        flash = GetComponent<Flash>();
        collider_enemy = GetComponent<Collider2D>();
        if (GetComponent<EnemyAI>())
        {enemyAI = GetComponent<EnemyAI>();

        }else if (GetComponent<EnemyStandAI>())
        {
            enemyStand= GetComponent<EnemyStandAI>();
        }
        
        
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth.Value = StartingHealth;
        knockback = GetComponent<KnockBack>();
        animator = GetComponent<Animator>();

        healthBar.maxValue = StartingHealth;
        healthBar.value = currentHealth.Value;
    //    numCurrentHealth.text = currentHealth.Value.ToString();

        // Subscribe to the OnValueChanged event to sync health across clients
        currentHealth.OnValueChanged += OnHealthChanged;
    }

/*    private void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }*/

    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        healthBar.value = newHealth;
      //  numCurrentHealth.text = newHealth.ToString();
    }

    private void Update()
    {
        if (!IsServer) { return; }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    [ServerRpc]
    public void TakeDamageServerRpc(float damage)
    {
        TakedDamage(damage);
    }

    public void TakedDamage(float damage)
    {
        if (!isInteractive) { return; }
        DetectDeath();
       
        if (isDead.Value) {
            Destroy(healthBarObject);
            return; }
        if (!knockback.GetKnockBack)
        {
            currentHealth.Value -= damage;
            if (enemyAI)
            {
                knockback.GettingKnockBack(enemyAI.target.transform, knockBackThrust);
            }
            else
            {
                knockback.GettingKnockBack(player.transform, knockBackThrust);
            }
           
            flash.TriggerFlashServerRpc();
        }


    }
    public void TakedDamageInIceBullet(float damage)
    {
        DetectDeath();
        if (isDead.Value)
        {
            Destroy(healthBarObject);
            return;
        }
        if (enemyPathFinding)
        {
        enemyPathFinding.isIceFreeze = true;
        }

     //   StartCoroutine(FreezeTime());
        if (!flash.takedDamage)
        {
            currentHealth.Value -= damage;
            flash.TriggerFlashServerRpc();
        }
        
    }
    public void IceBullet(float damage)
    {
        if (enemyAI)
        {
            enemyAI.isActive = false;
        }
        else if (enemyStand)
        {
            enemyStand.isActive = false;
        }
        if (enemyPathFinding)
        {
            enemyPathFinding.isIceFreeze = true;
        }

        StartCoroutine(FreezeTime());
    }
    private IEnumerator FreezeTime()
    {
        yield return new WaitForSeconds(3f);
        enemyPathFinding.isIceFreeze = false;
        enemyAI.isActive = true;
    }
    public void TakedDamageNotInPlayer(float damage,Transform transform_new)    {
        DetectDeath();

        if (isDead.Value)
        {
            Destroy(healthBarObject);
            return;
        }
        if (!knockback.GetKnockBack)
        {
            currentHealth.Value -= damage;
            knockback.GettingKnockBack(transform_new, knockBackThrust);
            flash.TriggerFlashServerRpc();
        }
    }
    private void DetectDeath()
    {
        if (currentHealth.Value <= 0&&isDead.Value==false)
        {
            collider_enemy.enabled = false;
            isDead.Value = true;
            StartCoroutine(PlayDeathAnimationEnemy());
          //  PlayDeathVFXClientRpc();
        }
    }

    private IEnumerator PlayDeathAnimationEnemy()
    {
        animator.SetTrigger("Death");
        if (dropItems_big && dropItems_medium && dropItems_small)
        {
            DropRandomItem();
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + addTimeAnim);
        
        Destroy(gameObject);
    }
    private void DropRandomItem()
    {
        if (!IsServer) return;
        int a=Random.Range(0, 15);
        if (a >= 0 && a <= 5)
        {
          GameObject dropItem =  Instantiate(dropItems_small,gameObject.transform.position,Quaternion.identity);
          dropItem.GetComponent<NetworkObject>().Spawn();
        }
        else if (a >= 6 && a <= 8)
        {
            GameObject dropItem= Instantiate(dropItems_medium, gameObject.transform.position, Quaternion.identity);
            dropItem.GetComponent<NetworkObject>().Spawn();
        }
        else if(a>=9&&a<=10)
        {
            GameObject dropItem= Instantiate(dropItems_big, gameObject.transform.position, Quaternion.identity);
            dropItem.GetComponent<NetworkObject>().Spawn();
        }
    }

/*    [ClientRpc]
    public void PlayDeathVFXClientRpc()
    {
        Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
    }*/
}
