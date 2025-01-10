using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapEndPortalPuzzle : NetworkBehaviour
{
    [SerializeField] private GameObject door_1;
    [SerializeField] private MapEnterPortalPuzzle room1;
    [SerializeField] private MapEnterPortalPuzzle room2;
    [SerializeField] private MapEnterPortalPuzzle room3;
    [SerializeField] private GameObject enemy_semi_boss;
    private bool isActiveBoss = false;
    private bool isDone = false;
    public override void OnNetworkSpawn()
    {
//door_1.GetComponent<Door>().isClose.Value = true;
    }
    void Start()
    {
        
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
        if(IsServer)
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
            if (isActiveBoss && !isDone)
            {
                isDone = true;
                DonDestroyClientRpc();
            }

    }
    [ClientRpc]
    public void DonDestroyClientRpc()
    {
        if (door_1 == null)
        {
            Debug.LogError("door_1 is null!");
            return;
        }

        var destroyComponent = door_1.GetComponent<DestroyGameObjectInAnimation>();
        if (destroyComponent == null)
        {
            Debug.LogError("DestroyGameObjectInAnimation component is missing on door_1!");
            return;
        }

        destroyComponent.DoneDestroy();
    }
}
