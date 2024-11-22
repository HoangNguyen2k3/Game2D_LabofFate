using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackBloomBoss : MonoBehaviour
{
    [SerializeField] private Transform postionDamage1;
    [SerializeField] private Transform bloom;

    public void CreateBloom()
    {
        Instantiate(bloom, postionDamage1.position, Quaternion.identity);
    }
}
