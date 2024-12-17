using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PortalManager : NetworkBehaviour
{
    public bool isActive = false;
    [SerializeField] private GameObject enemyPrefab; 
   // public List<GameObject> enemyPrefabList;
    [SerializeField] private float timeSpawn = 1f;
    public bool isSpawning = false;
    [SerializeField] private int num_enemySpawnMin = 0;
    [SerializeField] private int num_enemySpawnMax = 5;
    public bool doneSolve = false;
    [SerializeField] private int timesEnemySpawnOneTime = 5;
    private int current_spawnEnemy = 0;
    private bool wait_spawn = false;
    [SerializeField] private float wait_time_spawn = 15f;
    void Update()
    {
        if (doneSolve) { return; }
        if (wait_spawn) { return; }
        if (current_spawnEnemy >= timesEnemySpawnOneTime)
        {
            StartCoroutine(WaitSpawn());
        }
        else
        {
            if (IsServer && isActive && !isSpawning)
            {
                StartCoroutine(SpawnEnemy());
            }
        }

        
    }
    private IEnumerator WaitSpawn()
    {
        wait_spawn= true;
        current_spawnEnemy = 0;
        yield return new WaitForSeconds(wait_time_spawn);
        wait_spawn= false;
    }
    private IEnumerator SpawnEnemy()
    {
        current_spawnEnemy++;
        isSpawning = true;

        int num_enemy = Random.Range(num_enemySpawnMin, num_enemySpawnMax);
        for(int i=0; i<num_enemy; i++)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            NetworkObject networkObject = enemyInstance.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                networkObject.Spawn();
            }
        }


        yield return new WaitForSeconds(timeSpawn);
        isSpawning = false;
    }
}
