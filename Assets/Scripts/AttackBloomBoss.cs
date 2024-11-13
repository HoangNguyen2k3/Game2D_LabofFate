using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackBloomBoss : MonoBehaviour
{
    [SerializeField] private Transform postionDamage1;
    [SerializeField] private Transform postionDamage2;
    [SerializeField] private Transform bloom;
    private GameObject target;
    // Start is called before the first frame update

    public void CreateBloom()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target.transform.position.x > transform.position.x)
            {
                Instantiate(bloom, postionDamage1.position, Quaternion.identity);
            }
            else
            {
                Instantiate(bloom, postionDamage2.position, Quaternion.identity);
            }
        }

    }
}
