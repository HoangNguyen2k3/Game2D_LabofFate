using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public LineRenderer line;
    private Transform endPoint;
    public LayerMask layerMask;
    public Vector2 direction;

    public Dictionary<Vector2, List<Sprite>> dict = new Dictionary<Vector2, List<Sprite>>();
    [field: SerializeField] DirectionSprite directionSprite;
    public float distance = 10;
    // public bool active = false;
    public bool source = false;
    public bool attached = false;

    private void Awake()
    {
        line.useWorldSpace = true;

        if (directionSprite)
        {
            dict.Add(Vector2.up, new List<Sprite>() {directionSprite.inactiveSpriteUp, directionSprite.activeSpriteUp});
            dict.Add(Vector2.down, new List<Sprite>() {directionSprite.inactiveSpriteDown, directionSprite.activeSpriteDown});
            dict.Add(Vector2.left, new List<Sprite>() {directionSprite.inactiveSpriteLeft, directionSprite.activeSpriteLeft});
            dict.Add(Vector2.right, new List<Sprite>() {directionSprite.inactiveSpriteRight, directionSprite.activeSpriteRight});

            directionSprite.inactiveSprite = dict[direction][0];
            directionSprite.activeSprite = dict[direction][1];
        }
    }

    private void Start()
    {
        //
    }

    private void Update()
    {
        if (source)
        {
            Emitting();
            return;
        }

        // if (!source)
        // {
        //     StopEmitting();
        // }
    }

    public void Emitting()
    {
        // active = true;
        if(directionSprite)
        {
            directionSprite.Active();
        }
        line.enabled = true;
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, Mathf.Infinity , layerMask);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, (Vector2) this.transform.position + (direction * distance));

        if (ray.collider)
        {
            TargetBox t = ray.collider.GetComponent<TargetBox>();
            LaserEmitter l =  ray.collider.GetComponentInChildren<LaserEmitter>();
            line.SetPosition(1, ray.point);
            if (l)
            {
                // l.directionSprite.Active();
                l.Emitting();
            }
            if (t)
            {
                t.Active();
            }
        }
    }

    public void StopEmitting()
    {
        // active = false;
        if(directionSprite)
        {
            directionSprite.InActive();
        }
        line.enabled = false;
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, Mathf.Infinity , layerMask);
        if (ray.collider)
        {
            LaserEmitter l =  ray.collider.GetComponentInChildren<LaserEmitter>();
            TargetBox t = ray.collider.GetComponent<TargetBox>();
            if (l)
            {
                // l.directionSprite.InActive();
                l.StopEmitting();
            }
            if (t)
            {
                t.Inactive();
            }
        }
    }
}
