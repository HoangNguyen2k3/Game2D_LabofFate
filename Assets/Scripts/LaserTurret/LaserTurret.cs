using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    [SerializeField] private float distanceAttack = 5f;
    private Transform target;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;
    [SerializeField] private GameObject laser;
/*    [SerializeField] private float timeChangeTarget = 1f;
    private float timeChange = 0f;*/
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void Update()
    {
        if (ManagePuzzleRoom.Instance.current_crystal >= 4) { return; }
        if (canAttack&&Vector2.Distance(transform.position,target.position)< distanceAttack && ManagePuzzleRoom.Instance.PlayerInRange)
        {
            canAttack = false;
            Instantiate(laser, transform.position, Quaternion.identity);
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
