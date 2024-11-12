using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AimProjectile : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private GameObject bloom;
    [SerializeField] private float projectileRange = 10f;
    private Vector3 startPosition;
    [SerializeField] private float directStart = 0f;
    public float speedChange = 3f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
     
        MoveProjectile(Time.deltaTime);
        DetectFireDistance();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (indestructible || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")||player))
        {
            Instantiate(bloom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        if (Vector2.Distance(transform.position, startPosition) > projectileRange || moveSpeed <= 5)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile(float delta)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        Vector3 directionToPlayer = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 180f + directStart);


        transform.Translate(delta * moveSpeed * directionToPlayer, Space.World);

        if (moveSpeed > 5)
        {
            moveSpeed -= 0.01f;
        }
    }
}
