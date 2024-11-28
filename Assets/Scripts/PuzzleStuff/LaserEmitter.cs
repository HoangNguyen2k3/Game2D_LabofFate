using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public LineRenderer line;
    private Transform endPoint;
    public LayerMask layerMask;
    public Vector2 direction = Vector2.right;
    public float distance = 10;
    public bool active = false;

    private void Update()
    {
        if(active)
        {
            Emitting();
        }
        else line.enabled = false;

    }

    private void Emitting()
    {
        line.enabled = true;
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, Mathf.Infinity , layerMask);
        line.SetPosition(0, transform.position);
        Debug.Log(ray.point);
        if (ray)
        {
            line.SetPosition(1, ray.point);
            return;
        }
        line.SetPosition(1, (Vector2) this.transform.position + (direction * distance));
    }

}
