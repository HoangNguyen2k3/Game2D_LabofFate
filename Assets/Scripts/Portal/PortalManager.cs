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

    void Update()
    {
        if (IsServer && !isActive && !isSpawning)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        isSpawning = true;

        GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        NetworkObject networkObject = enemyInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); 
        }
        else
        {
            Debug.LogError("Prefab c?a quái v?t không có NetworkObject!");
        }

        yield return new WaitForSeconds(timeSpawn);
        isSpawning = false;
    }
}
