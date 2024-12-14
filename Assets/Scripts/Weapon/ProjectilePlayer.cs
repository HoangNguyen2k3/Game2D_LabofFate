using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePlayer : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particalOnHitPrefabVFX;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int damageAttack = 1;
    [SerializeField] private GameObject bullet;
    
    private Vector3 startPosition;
    private Vector3 moveDirection;
    [SerializeField] private bool isFireBullet = false;
    [SerializeField] private bool isIceBullet=false;
    [SerializeField] private bool isThunderBullet = false;
    [Header("Fire Bullet")]
    [SerializeField] private GameObject addObject;
    [Header("Thunder Bullet")]
    [SerializeField] private GameObject thunderAdd;
    [Header("Ice Bullet")]
    [SerializeField] private GameObject IceAdd;

/*    private void Start()
    {
        startPosition = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        moveDirection = (mousePos - startPosition).normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }*/
    public void Initialize(Vector3 direction)
    {
        startPosition = transform.position;
        moveDirection = direction.normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void Update()
    {
        if (IsServer)
        {
            MoveProjectile();
            DetectFireDistance();
        }
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.IsSpawned)
            {
                networkObject.Despawn();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) { return; }
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();

        if (!other.isTrigger && (enemyHealth || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            if (enemyHealth && isFireBullet)
            {
                enemyHealth.TakedDamage(damageAttack);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                GameObject instance = Instantiate(addObject, transform.position, Quaternion.identity);
                NetworkObject networkObject1 = instance.GetComponent<NetworkObject>();
                if (networkObject1 != null)
                {
                    networkObject1.Spawn(true);
                }
            }
            else if (enemyHealth && isIceBullet)
            {
                enemyHealth.TakedDamageInIceBullet(damageAttack);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Vector2 newPos = enemyHealth.transform.position;
                newPos.y -= 1.2f;
                GameObject instance = Instantiate(IceAdd, newPos, Quaternion.identity);
                NetworkObject networkObject1 = instance.GetComponent<NetworkObject>();
                if (networkObject1 != null)
                {
                    networkObject1.Spawn(true);
                }
            }
            else if (enemyHealth && isThunderBullet)
            {
                enemyHealth.TakedDamage(damageAttack);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                GameObject instance = Instantiate(thunderAdd, transform.position, Quaternion.identity);
                NetworkObject networkObject1 = instance.GetComponent<NetworkObject>();
                if (networkObject1 != null)
                {
                    networkObject1.Spawn(true);
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
            }


            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.IsSpawned)
            {
                networkObject.Despawn();
            }
        }
    }

    private void MoveProjectile()
    {

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
     
    }
}
