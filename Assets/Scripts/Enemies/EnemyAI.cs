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
    [SerializeField] private float speedRoaming = 2f;
    [SerializeField] private float speedFollow = 4f;
    [SerializeField] private float timeChangeTarget=1f;
    private float timeChange = 0f;

    [SerializeField] private bool dontMoveWhenAttack = false;

    private DirectionEnemy directionEnemy;

    public bool isActive = true;


    public bool followPlayer = false;

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
        directionEnemy = GetComponent<DirectionEnemy>();
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
/*    private void Start()
    {
        if(target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                target = player.transform;
            }
        }
    }*/
    void Update()
    {

        if (!IsServer) return;
        if(!isActive) { return; }
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                target = player.transform;
            }
        }
      
        if (timeChange < timeChangeTarget)
        {
            timeChange += Time.deltaTime;
        }
        else
        {
            timeChange = 0f;
            target = UpdateTargetPlayer();
        }
        if (health)
        {
            if (health.isDead.Value)
            {
                col.enabled = false;
                return;
            }
        }


        if (knockBack.GetKnockBack)
        {
            return;
        }
        MovementStateControl();
        if (state.Value == State.FollowPlayer || State.AttackPlayer == state.Value)
        {
            followPlayer = true;
        }
        else
        {
            followPlayer = false;
        }
        pathUpdateTimer += Time.deltaTime;
    }
    private Transform UpdateTargetPlayer(){
        if (target != null)
        {
            Transform newTranform = target;
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player"); 
            for(int i = 0;i < player.Length; i++)
            {
                if (Vector2.Distance(transform.position, player[i].transform.position) < 
                    Vector2.Distance(transform.position, newTranform.position))
                {
                    newTranform = player[i].transform;
                }
            }
           return newTranform;
        }
        return target;
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
        enemyPathFinding.moveSpeed = speedRoaming;
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
            if (IsServer)
            {
                if (roamPosition.Value.x > transform.position.x)
                {
                    /*                    if (directionEnemy.reverse)
                                        {
                                            Debug.Log(1);
                                            directionEnemy.SetFlipX(false);
                                        }
                                        else
                                        {
                                            Debug.Log(2);
                                            directionEnemy.SetFlipX(true);
                                        }*/
                //    Debug.Log(43);
                    directionEnemy.SetFlipX(true);
                }
                else
                {
                    /*                    if (directionEnemy.reverse)
                                        {
                                            Debug.Log(3);
                                            directionEnemy.SetFlipX(true);
                                        }
                                        else
                                        {
                                            Debug.Log(4);
                                            directionEnemy.SetFlipX(false);
                                        }*/
                 //   Debug.Log(44);
                    directionEnemy.SetFlipX(false);

                }
            }
      

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player==null)
        {
            state.Value = State.Roaming;
            return;
        }
        if (!canAttack)
        {
            state.Value = State.FollowPlayer;
        }
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
            if(!enemyType) {
                state.Value = State.FollowPlayer;
                return;
            }
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
            enemyPathFinding.moveSpeed = speedFollow;
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
        if (collision.gameObject.layer == 3)
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
        if (dontMoveWhenAttack)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        if (dontMoveWhenAttack)
        {
            rb.constraints=RigidbodyConstraints2D.FreezeRotation;
        }
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
