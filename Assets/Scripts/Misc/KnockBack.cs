using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KnockBack :  NetworkBehaviour
{
    public bool GetKnockBack {  get; private set; }
    private Rigidbody2D rb;
    [SerializeField] public float timeKnockBack = 0.2f;
    
    public bool canBeKnockback = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GettingKnockBack(Transform damageSoure,float knockbackThrust)
    {
        if (!canBeKnockback) return;
        GetKnockBack = true;
        Vector2 direction_knockBack = (-damageSoure.position + transform.position).normalized * knockbackThrust * rb.mass;
        rb.AddForce(direction_knockBack, ForceMode2D.Impulse);
        StartCoroutine(DoneKnockBack());
    }
    public void GettingKnockBack2(Vector3 damageSoure, float knockbackThrust)
    {
        if (!canBeKnockback) return;
        GetKnockBack = true;
        Vector2 direction_knockBack = (-damageSoure + transform.position).normalized * knockbackThrust * rb.mass;
        rb.AddForce(direction_knockBack, ForceMode2D.Impulse);
        StartCoroutine(DoneKnockBack());
    }
    private IEnumerator DoneKnockBack()
    {
        yield return new WaitForSeconds(timeKnockBack);
        rb.velocity = Vector2.zero;
        GetKnockBack = false;
    }
}
