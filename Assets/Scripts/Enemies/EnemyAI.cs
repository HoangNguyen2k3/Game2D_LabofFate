using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("A* Pathfinding")]
    public Seeker seeker;
    public Transform target;
    public Path path;
    public float nextWPDistance = 2f;  // Distance to the next waypoint
    private Rigidbody2D rb;
    private float updatePathInterval = 0.5f; // Frequency of path recalculation
    private float pathUpdateTimer;
    private bool isPathCalculating = false;  // Avoid concurrent path calculations
    private int currentWP = 0;
    private float stuckTime = 0f;
    private float stuckTimeMax = 1.5f;
    private Vector2 randomizedTargetOffset=Vector2.zero;

    [Header("EnemyAI")]
    [SerializeField] private float roamChangeDirFloat = 2f;
    private State state;
    private EnemyPathFinding enemyPathFinding;
    private float timeRoaming = 0f;
    private Vector2 roamPosition;
    private float rangeFollow = 20f;
    private KnockBack knockBack;
    private enum State
    {
        Roaming,
        FollowPlayer
    }

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        roamPosition = GetRoamingPosition();
        state = State.Roaming;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (knockBack.GetKnockBack)
        {
            return;
        }
        MovementStateControl();
        pathUpdateTimer += Time.deltaTime;
    }
    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.FollowPlayer:
                FollowingPlayer();
                break;
        }
    }
    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.moveSpeed = 2f;
        enemyPathFinding.MoveTo(roamPosition);

        if (target && Vector2.SqrMagnitude(transform.position- target.transform.position) <= rangeFollow*rangeFollow)
        {
            state = State.FollowPlayer;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
            timeRoaming = 0f; 
        }
    }

    private void FollowingPlayer()
    {
        if (!target || Vector2.Distance(transform.position, target.transform.position) > rangeFollow)
        {
            state = State.Roaming;
            return;
        }
         //Update path if timer allows and we are not already calculating it
         if (pathUpdateTimer >= updatePathInterval && !isPathCalculating)
         {
        CalculatePath();
       }
    }

    void CalculatePath()
    {
        isPathCalculating = true;
        pathUpdateTimer = 0f;
        seeker.StartPath(transform.position + (Vector3)randomizedTargetOffset.normalized, target.position , OnPathCallBack);
        randomizedTargetOffset = Vector2.zero;
    }

    void OnPathCallBack(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWP = 0;  // Reset waypoint index when new path is calculated
            MoveToTarget();
        }
        isPathCalculating = false;  // Path calculation finished
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

            if (Vector2.Distance(transform.position, target.transform.position) > rangeFollow)
            {
                state = State.Roaming;
                break;
            }

            yield return null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            stuckTime += Time.deltaTime;
        }
        if (stuckTime >= stuckTimeMax)
        {
            randomizedTargetOffset = transform.position- collision.transform.position;
            stuckTime = 0f;
        }
    }
}
