using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeathVFX : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (particle != null&&!particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
