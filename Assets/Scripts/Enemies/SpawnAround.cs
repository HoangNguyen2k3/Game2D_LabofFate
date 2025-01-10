using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAround : NetworkBehaviour
{
    [SerializeField] private LayerMask triggerLayer;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private float spawnRadius = 7f;
    [SerializeField] private GameObject enemy;
        public Vector2 GetValidSpawnPosition(Vector2 center)
    {
        int maxTempTest = 10;
        for(int i=0;i<maxTempTest; i++)
        {
            Vector2 randomPos = center + Random.Range(3f,spawnRadius)* Random.insideUnitCircle.normalized;
            Collider2D hit = Physics2D.OverlapCircle(randomPos, 0.1f, triggerLayer);
            Collider2D hit2 = Physics2D.OverlapCircle(randomPos, 0.1f, obstacles);
            if (hit != null&&hit2==null)
            {
                return randomPos;
            }
        }
        return center;
    }
    public void SpawnEnemies(int num)
    {
        if (IsServer)
        {
            for(int i=0;i< num;i++)
            {
                Vector2 temp = GetValidSpawnPosition(transform.position);
                GameObject gameObject = Instantiate(enemy, temp, Quaternion.identity);
                gameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }

}
