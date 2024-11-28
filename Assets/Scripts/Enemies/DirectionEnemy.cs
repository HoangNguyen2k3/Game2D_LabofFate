using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DirectionEnemy : NetworkBehaviour
{
    private SpriteRenderer bodyEnemy;
    private GameObject target;
    [SerializeField] private float distanceDetect;
    [SerializeField] private bool reverse = true;
    private EnemyAI enemyAI;

    // NetworkVariable to sync flipX state across the network
    public NetworkVariable<bool> flipXState = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        bodyEnemy = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        bodyEnemy.flipX = flipXState.Value;

        if (!IsServer) return; 
        HandleAI();
    }

    private void HandleAI()
    {
        if (!enemyAI.followPlayer) { return; }
        if (target != null)
        {
            if (enemyAI.target)
            {
                target.transform.position = enemyAI.target.position;
            }

            bool shouldFlip = reverse
                ? target.transform.position.x <= transform.position.x
                : target.transform.position.x > transform.position.x;

            SetFlipX(shouldFlip);
        }
        else
        {
            if (FindFirstObjectByType<PlayerController>())
            {
                target = FindFirstObjectByType<PlayerController>().gameObject;
            }
        }
    }

    private void SetFlipX(bool flip)
    {
        if (flipXState.Value != flip)
        {
            RequestFlipXStateServerRpc(flip); 
        }
    }

    [ServerRpc]
    private void RequestFlipXStateServerRpc(bool flip)
    {
        flipXState.Value = flip;
    }
}
