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
    [field: SerializeField] public TargetBox[] targetBoxList;

    [SerializeField] private GameObject informPlayerDone;

    public int areaActiveCount = 0;
    private int maxAreaCount;
    public int targetActiveCount = 0;
    private int maxTargetCount;
    private float time=0f;

    [SerializeField] private string levelSokoban="Sokoban1";
    [field: SerializeField] public bool AllAreaActive {get; private set;}

    private void Awake()
    {   
        informPlayerDone.SetActive(false);
        sokobanBoxList = GetComponentsInChildren<SokobanBox>();
        sokobanBoxAreaList = GetComponentsInChildren<SokobanBoxArea>();
        sokobanForceFieldsLists = GetComponentsInChildren<SokobanForceField>();
        targetBoxList = GetComponentsInChildren<TargetBox>();
    }

    private void Start()
    {
        maxAreaCount = sokobanBoxAreaList.Length;
        maxTargetCount = targetBoxList.Length;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time < 10f) { return; }
        if (AllAreaActive) return;

        if (areaActiveCount == maxAreaCount && targetActiveCount == maxTargetCount)
        {
            AllAreaActive = true;
            Debug.Log("You win");
            informPlayerDone.SetActive(true);
            GameObject puzzle = GameObject.FindGameObjectWithTag(levelSokoban);
            puzzle.GetComponent<Door>().isOpen.Value = true;
            DisableBoxesCollision();
            ActiveAllArea();
            return;
        }

        ForceFieldCheck();
        TargetCheck();
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


    public void TargetCheck()
    {
        targetActiveCount = 0;

        foreach (TargetBox t in targetBoxList)
        {
            if(t.active)
            {
                targetActiveCount ++;
            }
        }
    }

}
