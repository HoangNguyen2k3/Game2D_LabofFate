using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEndPortalPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject door_1;
    [SerializeField] private MapEnterPortalPuzzle room1;
    [SerializeField] private MapEnterPortalPuzzle room2;
    [SerializeField] private MapEnterPortalPuzzle room3;
    [SerializeField] private GameObject enemy_semi_boss;
    private bool isActiveBoss = false;
    private bool isDone = false;
    void Start()
    {
        door_1.GetComponent<Door>().isClose.Value = true;
        if (enemy_semi_boss)
        {
            enemy_semi_boss.GetComponent<EnemyAI>().isActive = false;
            enemy_semi_boss.GetComponent<EnemyHealth>().isInteractive = false;
            enemy_semi_boss.GetComponent<EnemyPathFinding>().isIceFreeze = true;
            enemy_semi_boss.GetComponent<SemiBoss>().Frezze();
        }

    }
    void Update()
    {
        if (enemy_semi_boss == null)
        {
            enemy_semi_boss = GameObject.FindGameObjectWithTag("SemiBoss");
            if (enemy_semi_boss)
            {
                enemy_semi_boss.GetComponent<EnemyAI>().isActive = false;
                enemy_semi_boss.GetComponent<EnemyHealth>().isInteractive = false;
                enemy_semi_boss.GetComponent<EnemyPathFinding>().isIceFreeze = true;
                enemy_semi_boss.GetComponent<SemiBoss>().Frezze();
            }
        }
        if (!isActiveBoss && room1.donePuzzle && room2.donePuzzle && room3.donePuzzle)
        {
            isActiveBoss = true;
            enemy_semi_boss.GetComponent<EnemyAI>().isActive = true;
            enemy_semi_boss.GetComponent<EnemyHealth>().isInteractive = true;
            enemy_semi_boss.GetComponent<EnemyPathFinding>().isIceFreeze = false;
            enemy_semi_boss.GetComponent<SemiBoss>().UnFrezze();
        }
        if (isActiveBoss && enemy_semi_boss == null&&!isDone)
        {
            isDone = true;
            door_1.GetComponent<Door>().isOpen.Value = true;
        }
    }
}
