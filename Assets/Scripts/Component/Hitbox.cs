using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [field: SerializeField] public Vector2 DamageRange {get; private set;} = Vector2.one;
    [field: SerializeField] public int CritChance {get; private set;} = 0;
    [field: SerializeField] public float CritMultiplier {get; private set;} = 1.5f;
    [field: SerializeField] public float KnockbackAmount {get; private set;} = 1f;
    public Collider2D hitArea;
    public LayerMask target;

    private void Awake()
    {
        CritChance = Mathf.Clamp(CritChance, 0, 100);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if ((target.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        if (other.TryGetComponent<Hurtbox>(out var hurtbox)) 
        {
            int damage = Random.Range((int)DamageRange.x, (int)DamageRange.y);

            if (Random.Range(1, 100) <= CritChance) 
            {
                damage = Mathf.CeilToInt(damage * CritMultiplier);
                Debug.Log("Critical Hit!");
            }

            hurtbox.TakeDamage(damage);
            hurtbox.TakeKnockback(transform, KnockbackAmount);
        }
    }

}
