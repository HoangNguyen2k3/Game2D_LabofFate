using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePlayerChildren : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particalOnHitPrefabVFX;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int damageAttack = 1;
    [SerializeField] private GameObject addObject;

    private Vector3 startPosition;
    private Vector3 moveDirection;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();

        if (!other.isTrigger && (enemyHealth || indestructible || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            if (enemyHealth)
            {
                enemyHealth.TakedDamage(damageAttack);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Instantiate(addObject, transform.position, transform.rotation);
                gameObject.GetComponent<NetworkObject>().Despawn(); Destroy(gameObject);
            }
            else if (indestructible || other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    private void MoveProjectile()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
