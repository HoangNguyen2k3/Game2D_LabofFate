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
    [SerializeField] private float timeChangeTarget = 1f;
    private float timeChange = 0f;
    public Transform target;
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
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    void Update()
    {

        if (timeChange < timeChangeTarget)
        {
            timeChange += Time.deltaTime;
        }
        else
        {
            timeChange = 0f;
            target = UpdateTargetPlayer();
        }
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
        if (player != null && Vector2.Distance(target.position, transform.position) < distanceAttack)
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
    private Transform UpdateTargetPlayer()
    {
            Transform newTranform = target;
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
            {
                if (Vector2.Distance(transform.position, player[i].transform.position) <
                    Vector2.Distance(transform.position, newTranform.position))
                {
                    newTranform = player[i].transform;
                }
            }
            return newTranform;
        }
}
