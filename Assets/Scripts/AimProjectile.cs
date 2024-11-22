using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AimProjectile : NetworkBehaviour
{
    public float initialSpeed = 20f;
    public float minSpeed = 4f;
    public float speedChange = 1f;
    public float degChange = 180f;
    [SerializeField] private GameObject bloom;
    
    private float moveSpeed;

    public float lifeTime = 5f;
    public float delayChaseTime = 0.5f;
    private float timer = 0;

    public Vector2 initialVelocity = Vector2.up;
    private Vector2 moveVelocity;

    private void Start() {
        moveVelocity = initialVelocity;
        moveSpeed = initialSpeed;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Explode();
        }

        if (moveSpeed > minSpeed)
        {
            moveSpeed -= speedChange;
        }
        transform.right = -moveVelocity;
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructive indestructible = other.gameObject.GetComponent<Indestructive>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // if (!other.isTrigger && (indestructible || other.gameObject.layer == LayerMask.NameToLayer("Obstacles")||player))
        // {
        //     Explode();
        // }
        if (player)
        {
            Explode();
        }
    }

    private void MoveProjectile()
    {
        Vector2 currentPosition = transform.position;
        if (timer > delayChaseTime && GameObject.FindGameObjectWithTag("Player")) 
        {
            Vector2 targetPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
            Vector2 directionToPlayer = (targetPosition - currentPosition).normalized;
            
            float angle = Vector2.SignedAngle(moveVelocity, directionToPlayer);

            moveVelocity = rotate(moveVelocity, ((angle > 0f) ? 1f:-1f) * degChange * Time.deltaTime);
        }

        transform.Translate(Time.deltaTime * moveSpeed * moveVelocity, Space.World);
    }

    private Vector2 rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    private void Explode()
    {
        if (bloom) Instantiate(bloom, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
