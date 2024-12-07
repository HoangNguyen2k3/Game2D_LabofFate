using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3ArmLaserBeam : MonoBehaviour
{
    public int laserDamage = 1;
    private LineRenderer lineRenderer;
    public LayerMask layerMask;
    public float maxDistance = 30;
    private bool active;
    private Boss3Arm bossArm;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bossArm = GetComponentInParent<Boss3Arm>();
    }

    private void Start()
    {
        lineRenderer.useWorldSpace = true;
        Disable();
    }

    private void Update() 
    {
        if (!active) return;
        
        Vector3 direction = bossArm.transform.localScale.x == 1 ? transform.right : -transform.right;

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction * maxDistance);
        lineRenderer.enabled = true;
        
        if (rayHit && rayHit.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = rayHit.collider.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.TakedDamageToPlayer(laserDamage, playerHealth.transform);
        }
    }

    public void Enable()
    {
        active = true;
    }

    public void Disable()
    {
        active = false;
        lineRenderer.enabled = false;
    }
}
