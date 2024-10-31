using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private KnockBack knockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }
    private void FixedUpdate()
    {
        if (knockBack.GetKnockBack) {
            Debug.Log("not move");
            return; 
        }
        rb.MovePosition(rb.position+moveDir.normalized*moveSpeed*Time.deltaTime);
         //rb.AddForce(moveDir.normalized * moveSpeed );
       // rb.velocity = moveDir.normalized * moveSpeed ;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(WaitToReverse());
    }
    private IEnumerator WaitToReverse()
    {
        yield return new WaitForSeconds(0.5f);
        moveDir *= (-1);
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
