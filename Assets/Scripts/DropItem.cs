using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DropItem : NetworkBehaviour
{
    private Transform target;
    [SerializeField] private int numAddHealth=1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = UpdateTargetPlayer();
            player.GetComponent<PlayerHealth>().HealingPlayerHealthServerRpc(numAddHealth);           
            if (IsServer)
            {
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
            else { Destroy(gameObject); }
            
        }

    }
    private GameObject UpdateTargetPlayer()
    {
        
        GameObject targetGameObject = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null)
        {
            Transform newTranform = target;
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
            {
                if (Vector2.Distance(transform.position, player[i].transform.position) <
                    Vector2.Distance(transform.position, newTranform.position))
                {
                    newTranform = player[i].transform;
                    targetGameObject = player[i];
                }
            }
            return targetGameObject;
        }
        return targetGameObject;
    }
}
