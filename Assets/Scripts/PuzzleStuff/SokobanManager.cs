using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class SokobanManager : MonoBehaviour
{

    [field: SerializeField] public SokobanBox[] sokobanBoxList;
    [field: SerializeField] public SokobanBoxArea[] sokobanBoxAreaList;

    public int areaActiveCount = 0;
    private int maxAreaCount;

    [field: SerializeField] public bool AllAreaActive {get; private set;}

    private void Awake()
    {
        sokobanBoxList = GetComponentsInChildren<SokobanBox>();
        sokobanBoxAreaList = GetComponentsInChildren<SokobanBoxArea>();
    }

    private void Start()
    {
        maxAreaCount = sokobanBoxAreaList.Length;
    }

    private void Update()
    {
        if (AllAreaActive) return;

        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }

        if (areaActiveCount == maxAreaCount)
        {
            AllAreaActive = true;
            Debug.Log("You win");
            DisableBoxesCollision();
            ActiveAllArea();
            return;
        }
    }

    private void DisableBoxesCollision()
    {
        foreach (SokobanBox s in sokobanBoxList)
        {
            s.DisableHitbox();
        }
    }

    private void ActiveAllArea()
    {
        foreach (SokobanBoxArea s in sokobanBoxAreaList)
        {
            s.Active();
        }
    }

    private void Reset()
    {
        foreach (SokobanBox s in sokobanBoxList)
        {
            s.Reset();
        }
    }

}
