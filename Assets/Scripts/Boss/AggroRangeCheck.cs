using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRangeCheck : MonoBehaviour
{
    private GameObject player;
    private Boss boss;

    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GetComponentInParent<Boss>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == player)
        {
            boss.SetAggroRangeCheck(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player)
        {
            boss.SetAggroRangeCheck(false);
        }
    }
}
