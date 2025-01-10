using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRangeCheck : MonoBehaviour
{
    private GameObject player;
    private BossCore boss;
    [SerializeField] private TargetChange targetChange;
    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GetComponentInParent<BossCore>();
        targetChange.OnTargetChanged += UpdateTarget;
    }
    private void UpdateTarget(GameObject newTarget)
    {
        player = newTarget;
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
