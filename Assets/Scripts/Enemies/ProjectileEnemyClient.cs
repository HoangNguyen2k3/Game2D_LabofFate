using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyClient : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particalOnHitPrefabVFX;
    [SerializeField] public bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPosition;

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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && (other.gameObject.GetComponent<PlayerHealth>() ||
                                 other.gameObject.GetComponent<Indestructive>() ||
                                 other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakedDamageToPlayerKnockBack(1, transform);
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }
    private void MoveProjectile()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.right);
    }
}
