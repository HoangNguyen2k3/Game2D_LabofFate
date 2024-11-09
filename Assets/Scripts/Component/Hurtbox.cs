using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [field: SerializeField] public Health Health {get; private set;}
    [SerializeField] private bool canBeKnockback = false;
    

    private void Awake()
    {   
        Health = GetComponentInParent<Health>();
    }

    public void TakeDamage(float damageAmount)
    {
        Health.TakeDamage(damageAmount);
    }

    public void TakeKnockback(Transform knockbackForce, float knockbackAmount)
    {
        if (!canBeKnockback) return;
        if (transform.parent.TryGetComponent<Rigidbody2D>(out var body)) 
        {
            Vector2 knockback = transform.position - knockbackForce.position;
            if (body.velocity != Vector2.zero)
            {
                knockback -= body.velocity;
            }

            knockback = knockback.normalized * knockbackAmount;
            body.velocity = knockback; 
        }
    }
}
