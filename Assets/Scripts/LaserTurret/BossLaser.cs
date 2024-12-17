using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] private float distanceAttack = 10f;
    private Transform target;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;
    [SerializeField] private GameObject laser;
    private bool startPharse=false;
    [SerializeField] private GameObject bloom;
    
    private void Start()
    {
        if (target == null&& GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void Update()
    {
        if (target == null&& GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (ManagePuzzleRoom.Instance.current_crystal < 4)
        {
            return;
        }
        if (canAttack && Vector2.Distance(transform.position, target.position) < distanceAttack && ManagePuzzleRoom.Instance.PlayerInRange)
        {
            canAttack = false;
            Instantiate(laser, transform.position, Quaternion.identity);
            StartCoroutine(AttackCooldownRoutine());
        }
        if (startPharse) { return; }
        if (ManagePuzzleRoom.Instance.current_crystal >= 4) { 

            startPharse = true;

            StartCoroutine(StartProcess());

            return; 
        }

    }
    private IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private IEnumerator StartProcess()
    {
        yield return new WaitForSeconds(30f);
        Instantiate(bloom, transform.position, Quaternion.identity);
        GameObject puzzle = GameObject.FindGameObjectWithTag("StonePuzzle");
        puzzle.GetComponent<Door>().isOpen.Value = true;
        Destroy(gameObject);
    }
}
