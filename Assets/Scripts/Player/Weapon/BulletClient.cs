using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletClient : MonoBehaviour
{
    [SerializeField] private ArbaletScriptaleObject elementalBullet;

    private Vector3 startPosition;
    private Vector3 moveDirection;
    public void Initialize(Vector3 direction)
    {
        startPosition = transform.position;
        moveDirection = direction.normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }
    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > elementalBullet.projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (!other.isTrigger && (enemyHealth || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            if (enemyHealth && elementalBullet.nameElemental == "fire")
            {
                Instantiate(elementalBullet.particalOnHitPrefabVFX, transform.position, transform.rotation);
            }
            else if (enemyHealth && elementalBullet.nameElemental == "ice")
            {
                Instantiate(elementalBullet.particalOnHitPrefabVFX, transform.position, transform.rotation);
            }
            else if (enemyHealth && elementalBullet.nameElemental == "thunder")
            {
                Instantiate(elementalBullet.particalOnHitPrefabVFX, transform.position, transform.rotation);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Instantiate(elementalBullet.particalOnHitPrefabVFX, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        
    }

    private void MoveProjectile()
    {
        transform.position += moveDirection * elementalBullet.moveSpeed * Time.deltaTime;
    }
}
