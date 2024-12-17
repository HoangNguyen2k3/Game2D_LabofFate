using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PortalManager : NetworkBehaviour
{
    public bool isActive = false;
    [SerializeField] private GameObject enemyPrefab; 
    [SerializeField] private float timeSpawn = 1f;
    public bool isSpawning = false;
    [SerializeField] private int num_enemySpawnMin = 0;
    [SerializeField] private int num_enemySpawnMax = 5;
    public bool doneSolve = false;
    void Update()
    {
        if (doneSolve) { return; }
        if (IsServer && isActive && !isSpawning)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
       
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
