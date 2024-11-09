using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Pathfinding;

public class EnemyAI : NetworkBehaviour
{
    [Header("A* Pathfinding")]
    public Seeker seeker;
    public Transform target;
    private Path path;
    public float nextWPDistance = 2f;
    private float updatePathInterval = 0.5f;
    private float pathUpdateTimer;
    private bool isPathCalculating = false;
    private int currentWP = 0;
    private float stuckTime = 0f;
    private float stuckTimeMax = 1.5f;
    private Vector2 randomizedTargetOffset = Vector2.zero;

    [Header("EnemyAI")]
    [SerializeField] private float roamChangeDirFloat = 2f;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Roaming, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private EnemyPathFinding enemyPathFinding;
    private float timeRoaming = 0f;
    private NetworkVariable<Vector2> roamPosition = new NetworkVariable<Vector2>();
   [SerializeField] private float rangeFollow = 8f;
    [SerializeField] private float rangeAttack = 1f;
    private KnockBack knockBack;
    private EnemyHealth health;
    private Collider2D col;
    private Rigidbody2D rb;
    [SerializeField] private MonoBehaviour enemyType;
    private bool canAttack = true;
    [SerializeField] private float attackCooldown = 0.5f;

    private enum State
    {
        Roaming,
        FollowPlayer,
        AttackPlayer
    }

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        roamPosition.Value = GetRoamingPosition();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        col = GetComponent<Collider2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer && target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                target = player.transform;
            }
        }
    }

    void Update()
    {

        if (!IsServer) return;
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                target = player.transform;
            }
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

        MovementStateControl();
        pathUpdateTimer += Time.deltaTime;
    }

    private void MovementStateControl()
    {
        switch (state.Value)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.FollowPlayer:
                if (target == null) { state.Value = State.Roaming; break; }
                FollowingPlayer();
                break;
            case State.AttackPlayer:
                AttackPlayer();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.moveSpeed = 2f;
        enemyPathFinding.MoveTo(roamPosition.Value);

//        if (target && CaculateDistancePosition(transform.position,target.position,rangeFollow) && !CaculateDistancePosition(transform.position,target.position,rangeAttack))
            if (target && Vector2.Distance(transform.position,target.position)<=rangeFollow
            && Vector2.Distance(transform.position,target.position)>rangeAttack)

            {
                state.Value = State.FollowPlayer;
        }
        //else if(target&& CaculateDistancePosition(transform.position, target.position, rangeAttack))
        else if (target && Vector2.Distance(transform.position,target.position)<=rangeAttack)
        {
            state.Value = State.AttackPlayer;
        
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition.Value = GetRoamingPosition();
            timeRoaming = 0f;
        }
    }

    private void FollowingPlayer()
    {
        if (target == null || (Vector2.Distance(transform.position, target.position) > rangeFollow))
        {
            state.Value = State.Roaming;
            return;
        }else if (target!=null&&Vector2.Distance(transform.position, target.position) <= rangeAttack)
        {
            state.Value = State.AttackPlayer;
            return;
        }

        if (pathUpdateTimer >= updatePathInterval && !isPathCalculating)
        {
            CalculatePath();
        }
    }
    private void AttackPlayer()
    {
        if (!canAttack)
        {
            state.Value = State.FollowPlayer;
        }
        Debug.Log("Attack");
        if (target == null || (Vector2.Distance(transform.position, target.position) > rangeFollow))
        {
            state.Value = State.Roaming;
            return;
        }
        else if (target != null && Vector2.Distance(transform.position, target.position) > rangeAttack&&Vector2.Distance(transform.position, target.position) <= rangeAttack)
        {
            state.Value = State.FollowPlayer;
            return;
        }
        if (canAttack)
        {
            Debug.Log("Attack done");
            canAttack = false;
            (enemyType as IEnemy).Attack();
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    void CalculatePath()
    {
        isPathCalculating = true;
        pathUpdateTimer = 0f;
        seeker.StartPath(transform.position + (Vector3)randomizedTargetOffset.normalized, target.position, OnPathCallBack);
        randomizedTargetOffset = Vector2.zero;
    }

    void OnPathCallBack(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWP = 0;
            MoveToTarget();
        }
        isPathCalculating = false;
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void MoveToTarget()
    {
        if (currentWP < path.vectorPath.Count)
        {
            StopCoroutine("MoveToTargetCoroutine");
            StartCoroutine(MoveToTargetCoroutine());
        }
    }

    IEnumerator MoveToTargetCoroutine()
    {
        while (currentWP < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - rb.position).normalized;
            enemyPathFinding.moveSpeed = 6f;
            enemyPathFinding.MoveTo(direction);

            float distanceToWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWP]);

            if (distanceToWaypoint < nextWPDistance)
            {
                currentWP++;
            }

            if (target != null && Vector2.Distance(transform.position, target.position) > rangeFollow)
            {
                state.Value = State.Roaming;
                break;
            }

            yield return null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            stuckTime += Time.deltaTime;
        }
        if (stuckTime >= stuckTimeMax)
        {
            randomizedTargetOffset = (Vector2)transform.position - (Vector2)collision.transform.position;
            stuckTime = 0f;
        }
    }
    private IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private bool CaculateDistancePosition(Vector3 start,Vector3 stop,float distance)
    {
        if(Vector2.SqrMagnitude((Vector2)start - (Vector2)stop) <= distance * distance)
        {
            return true;
        }
        return false;
        
    }
}
