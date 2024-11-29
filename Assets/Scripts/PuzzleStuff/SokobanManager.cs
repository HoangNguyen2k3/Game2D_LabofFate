using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.AI;

public class SokobanManager : MonoBehaviour
{

    [field: SerializeField] public SokobanBox[] sokobanBoxList;
    [field: SerializeField] public SokobanBoxArea[] sokobanBoxAreaList;
    [field: SerializeField] public SokobanForceField[] sokobanForceFieldsLists;

    public int areaActiveCount = 0;
    public int maxAreaCount;
    private float time=0f;

    [SerializeField] private string levelSokoban="Sokoban1";
    [field: SerializeField] public bool AllAreaActive {get; private set;}

    private void Awake()
    {
        sokobanBoxList = GetComponentsInChildren<SokobanBox>();
        sokobanBoxAreaList = GetComponentsInChildren<SokobanBoxArea>();
        sokobanForceFieldsLists = GetComponentsInChildren<SokobanForceField>();
    }

    private void Start()
    {
        maxAreaCount = sokobanBoxAreaList.Length;
    }

    private void Update()
    {
        Debug.Log("wtf");
        Debug.Log(areaActiveCount);
        Debug.Log(maxAreaCount);
        time += Time.deltaTime;
        if (time < 120f) { return; }
        if (AllAreaActive) return;

        if (areaActiveCount == maxAreaCount)
        {
            AllAreaActive = true;
            Debug.Log("You win");
            GameObject puzzle = GameObject.FindGameObjectWithTag(levelSokoban);
            puzzle.GetComponent<Door>().isOpen.Value = true;
            DisableBoxesCollision();
            ActiveAllArea();
            return;
        }

        ForceFieldCheck();
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

    private void ForceFieldCheck()
    {
        foreach (SokobanForceField s in sokobanForceFieldsLists)
        {
            if(s.Triggered())
            {
                s.isTriggered = false;
                Reset();
            }
        }
    }
   

}
