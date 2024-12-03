using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PortalManager : NetworkBehaviour
{
    public bool isActive=false;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float timeSpawn = 1f;
    public bool isSpawning = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive&&!isSpawning)
        {
            SpawnEnemy();
        }
    }
    private IEnumerator SpawnEnemy()
    {
        isSpawning = true;
        Instantiate(enemy, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeSpawn);
        isSpawning=false;
    }
}
