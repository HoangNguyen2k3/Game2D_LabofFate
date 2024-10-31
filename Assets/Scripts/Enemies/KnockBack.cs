using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool GetKnockBack {  get; private set; }
    private Rigidbody2D rb;
    [SerializeField] public float timeKnockBack = 0.2f;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GettingKnockBack(Transform damageSoure,float knockbackThrust)
    {
        GetKnockBack = true;
        Vector2 direction_knockBack = (-damageSoure.position + transform.position).normalized * knockbackThrust * rb.mass;
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
