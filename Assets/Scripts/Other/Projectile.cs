using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particalOnHitPrefabVFX;
    [SerializeField] public bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPosition;

    public bool usingPoolObject = false;

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
  /*  private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (player || indestructible || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        {
            Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);

           if (IsServer)
            {
                if (usingPoolObject)
                {
                    GetComponentInParent<ObjectPoolingManager>().ReturnObject(gameObject);
                    //  FindObjectOfType<ObjectPoolingManager>().ReturnObject(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                gameObject.SetActive(false);
                GetComponentInParent<ObjectPoolingManager>().ReturnObject(gameObject);
            }

        }
      *//*  if (!other.isTrigger && (enemyHealth || indestructible || player))
        {
          *//*  if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {

                player?.TakeDamage(1, transform);
                Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }*//*
            if (!other.isTrigger && indestructible)
            {

               // Instantiate(particalOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
   *//*
    }
*/
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
                playerHealth.TakedDamageToPlayer(1, transform);
            }
            if (IsServer)
            {

                if (usingPoolObject)
                {
                    GetComponentInParent<ObjectPoolingManager>().ReturnObject(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            if (IsServer)
            {
                if (usingPoolObject)
                {
                    GetComponentInParent<ObjectPoolingManager>().ReturnObject(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    private void MoveProjectile()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.right);
    }
}
