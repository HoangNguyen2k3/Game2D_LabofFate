using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackBloomBoss : MonoBehaviour
{
    [SerializeField] private Transform postionDamage;
    [SerializeField] private Transform postionDamage1;
    [SerializeField] private Transform bloom;

    private Boss boss;
    private void Awake() 
    {
        boss = GetComponentInParent<Boss>();
    }
    public void CreateBloom()
    {

        Transform pos = (boss.GetDirToTarget().x < 0) ? postionDamage : postionDamage1;
        Instantiate(bloom, pos.position, Quaternion.identity);
    }
}
