using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public LineRenderer line;
    private Transform endPoint;
    public LayerMask layerMask;
    public Vector2 direction;
    public Vector3 position;
    public float distance = 10;
    public bool active = false;
    public bool source = false;

    private void Awake()
    {
        line.useWorldSpace = true;
    }

    private void Update()
    {
        if(active&&source)
        {
            Emitting();
            return;
        }
        line.enabled = false;
        if(source)
        {
            StopEmitting();
        }
    }

    public void Emitting()
    {
        active = true;
        line.enabled = true;
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, Mathf.Infinity , layerMask);
        line.SetPosition(0, transform.position);
        // Debug.Log(ray.point);
        line.SetPosition(1, (Vector2) this.transform.position + (direction * distance));
        if (ray.collider)
        {
            line.SetPosition(1, ray.point);
            if(ray.collider.GetComponentInChildren<LaserEmitter>())
            {
                ray.collider.GetComponentInChildren<LaserEmitter>().Emitting();
            }
        }
    }

    public void StopEmitting()
    {
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, Mathf.Infinity , layerMask);
        if (ray.collider)
        {
            active = false;
            if (ray.collider.GetComponentInChildren<LaserEmitter>())
            {
                ray.collider.GetComponentInChildren<LaserEmitter>().active = false;
                ray.collider.GetComponentInChildren<LaserEmitter>().StopEmitting();
            }

            if(ray.collider.GetComponent<TargetBox>())
            {
                ray.collider.GetComponent<TargetBox>().active = true;
            }
        }
    }
}
