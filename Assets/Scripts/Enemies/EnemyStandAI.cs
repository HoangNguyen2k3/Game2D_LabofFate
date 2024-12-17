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

    public bool isActive = true;
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


        if (!IsServer) return;
        if (!isActive) return;
        if (timeChange < timeChangeTarget)
        {
            timeChange += Time.deltaTime;
        }
        else
        {
            timeChange = 0f;
            target = UpdateTargetPlayer();
        }
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
/*        GameObject player = null;
        if (FindFirstObjectByType<PlayerController>())
        {
            player = FindFirstObjectByType<PlayerController>().gameObject;
        }*/
    if(target == null) { return; }
        if (Vector2.Distance(target.position,transform.position)<distanceAttack)
        {
            state.Value= State.AttackPlayer;
        }


    }
    public void AttackPlayer()
    {

        if (!target) return;
        if (Vector2.Distance(target.position, transform.position) < distanceAttack)
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return null; 
        }

        Transform closestTransform = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestTransform = player.transform;
            }
        }

        return closestTransform ?? target;
    }

}
