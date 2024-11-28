using System.Collections;
using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : NetworkBehaviour
{
    KnockBack knockback;
    [SerializeField] private float StartingHealth;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private GameObject player;
    [SerializeField] private float knockBackThrust = 15f;
    private Animator animator;
    private EnemyAI enemyAI;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject deathVFXPrefab;
  //  [SerializeField] private TextMeshProUGUI numCurrentHealth;
  //  [SerializeField] private TextMeshProUGUI name_enemy;
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);
    private Flash flash;

    [SerializeField] private float addTimeAnim=1f;

    [SerializeField] private GameObject dropItems_small;
    [SerializeField] private GameObject dropItems_medium;
    [SerializeField] private GameObject dropItems_big;

    void Start()
    {
        flash = GetComponent<Flash>();
        enemyAI = GetComponent<EnemyAI>();
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
    {      DetectDeath();
       
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

    private void DetectDeath()
    {
        if (currentHealth.Value <= 0)
        {
            isDead.Value = true;
            StartCoroutine(PlayDeathAnimationEnemy());
          //  PlayDeathVFXClientRpc();
        }
    }

    private IEnumerator PlayDeathAnimationEnemy()
    {
        animator.SetTrigger("Death");
        DropRandomItem();
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
