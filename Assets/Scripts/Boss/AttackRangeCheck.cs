using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackRangeCheck : MonoBehaviour
{
    private GameObject player;
    private Boss boss;

    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GetComponentInParent<Boss>();
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == player)
        {
            boss.SetAttackRangeStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player)
        {
            boss.SetAttackRangeStatus(false);
        }
    }
}
