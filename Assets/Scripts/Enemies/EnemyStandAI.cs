using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Pathfinding;

public class EnemyStandAI : NetworkBehaviour
{
    [Header("EnemyStandAI")]
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private KnockBack knockBack;
    private EnemyHealth health;
    private Collider2D col;
    [SerializeField] private float distanceAttack = 10f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown=1f;
    private bool canAttack = true;
    private enum State
    {
        Idle,
        AttackPlayer
    }

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        health = GetComponent<EnemyHealth>();
        col = GetComponent<Collider2D>();
    }
    void Update()
    {

        if (!IsServer) return;
        if (health.isDead.Value)
        {
            col.enabled = false;
            return;
        }

        if (knockBack.GetKnockBack)
        {
            return;
        }
        StateControl();

    }
    private void StateControl()
    {
        switch (state.Value)
        {
            case State.Idle:
                IdleState();
                break;
            case State.AttackPlayer:
                AttackPlayer();
                break;
        }
    }
    private void IdleState()
    {
        GameObject player = null;
        if (FindFirstObjectByType<PlayerController>())
        {
            player = FindFirstObjectByType<PlayerController>().gameObject;
        }
        if (player != null&&Vector2.Distance( player.transform.position,transform.position)<distanceAttack)
        {
            state.Value= State.AttackPlayer;
        }


    }
    public void AttackPlayer()
    {

        GameObject player = null;
        if (FindFirstObjectByType<PlayerController>())
        {
            player = FindFirstObjectByType<PlayerController>().gameObject;
        }        
        if (player != null && Vector2.Distance(player.transform.position, transform.position) < distanceAttack)
        {
            if (canAttack)
            {
                canAttack = false;
                (enemyType as IEnemy).Attack();
                StartCoroutine(AttackCooldownRoutine());
            }

        }
        else
        {
            state.Value = State.Idle;
        }

    }
    private IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
