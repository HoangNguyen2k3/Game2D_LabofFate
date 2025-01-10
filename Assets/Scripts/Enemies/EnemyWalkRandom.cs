using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyWalkRandom : NetworkBehaviour
{
    private float timeRoaming = 0f;
    private Vector2 moveDir;
    private NetworkVariable<Vector2> roamPosition = new NetworkVariable<Vector2>();
    [SerializeField] private float roamChangeDirFloat = 2f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] public float moveSpeed = 5f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        MoveTo(roamPosition.Value);
    }

    void Update()
    {
        if (!IsServer) return;
       Roaming();
    }
    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
       
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition.Value = GetRoamingPosition();
            MoveTo(roamPosition.Value);
            timeRoaming = 0f;
            if (roamPosition.Value.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }
    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakedDamageToPlayer(1,transform);
        }
        StartCoroutine(WaitToReverse());
    }
    private IEnumerator WaitToReverse()
    {
        yield return new WaitForSeconds(0.2f);
        moveDir *= (-1);
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    public void MoveTo(Vector2 moveDirection)
    {
        moveDir = moveDirection;
    }
    public void StopMove()
    {
        moveDir = Vector2.zero;
    }

}
