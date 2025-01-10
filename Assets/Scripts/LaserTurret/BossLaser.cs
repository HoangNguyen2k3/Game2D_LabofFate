using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossLaser : NetworkBehaviour
{
    [SerializeField] private float distanceAttack = 10f;
    private GameObject target;
    [SerializeField] private float attackCooldown = 1f;
    private bool canAttack = true;
    [SerializeField] private GameObject laser;
    private bool startPharse=false;
    [SerializeField] private GameObject bloom;
    private TargetChange targetChange;
    
    private void Start()
    {
        targetChange = GetComponent<TargetChange>();
        targetChange.OnTargetChanged += UpdateTarget;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void UpdateTarget(GameObject newTarget)
    {
        target = newTarget;
    }
    private void Update()
    {

        if (ManagePuzzleRoom.Instance.current_crystal.Value < 4)
        {
            return;
        }
        if (canAttack && Vector2.Distance(transform.position, target.transform.position) < distanceAttack && ManagePuzzleRoom.Instance.PlayerInRange)
        {
            canAttack = false;

            if (IsServer)
            {
 GameObject laser_new = Instantiate(laser, transform.position, Quaternion.identity);
            laser_new.GetComponent<NetworkObject>().Spawn();
            }
           
            StartCoroutine(AttackCooldownRoutine());
        }
        if (startPharse) { return; }
        if (ManagePuzzleRoom.Instance.current_crystal.Value >= 4) { 

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
