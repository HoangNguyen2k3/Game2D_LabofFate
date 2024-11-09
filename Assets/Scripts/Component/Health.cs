using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth {get; private set;}
    [field: SerializeField] public float CurrentHealth {get; private set;}

    private void Start() 
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " Died!");
        Destroy(gameObject);
    }

}
