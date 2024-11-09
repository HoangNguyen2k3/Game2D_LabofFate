using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GiveDamageToPlayer : NetworkBehaviour
{
    [SerializeField] private int damage=1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakedDamageToPlayer(damage,transform);
        }
    }
}
