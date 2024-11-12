using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particalOnHitPrefabVFX;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int damageAttack = 1;
    [SerializeField] private GameObject bullet;

    private Vector3 startPosition;
    private Vector3 moveDirection;

    private void Awake()
    {
        startPosition = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        moveDirection = (mousePos - startPosition).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        SpawnAdditionalBullets(angle);
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
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();
        if (!other.isTrigger && (enemyHealth || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            if (enemyHealth)
            {
                enemyHealth.TakedDamage(damageAttack);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if ( other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Debug.Log("Hello");
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
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
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void SpawnAdditionalBullets(float baseAngle)
    {
        float angleOffset1 = baseAngle + 15f;
        float angleOffset2 = baseAngle - 15f;

        // Calculate directions for additional bullets
        Vector3 direction1 = new Vector3(Mathf.Cos(angleOffset1 * Mathf.Deg2Rad), Mathf.Sin(angleOffset1 * Mathf.Deg2Rad), 0f).normalized;
        Vector3 direction2 = new Vector3(Mathf.Cos(angleOffset2 * Mathf.Deg2Rad), Mathf.Sin(angleOffset2 * Mathf.Deg2Rad), 0f).normalized;

        // Instantiate bullets and set their movement direction
        GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angleOffset1));
        GameObject bullet2 = Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angleOffset2));

        bullet1.GetComponent<ProjectilePlayerChildren>().SetMoveDirection(direction1);
        bullet2.GetComponent<ProjectilePlayerChildren>().SetMoveDirection(direction2);
    }
}
