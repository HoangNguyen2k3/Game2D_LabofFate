using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManagerGameStartScene : MonoBehaviour
{
    public static string PlayerName { get; set; }

    [SerializeField] private List<Transform> playerSpawnPositions; // List of predefined player spawn positions
    [SerializeField] private List<GameObject> typeEnemySpawn;
    [SerializeField] private List<Transform> positionSpawn;

    public void SpawnEnemies()
    {
        for (int i = 0; i < positionSpawn.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn[i], positionSpawn[i].position, Quaternion.identity);
            spawnEnemy.GetComponent<NetworkObject>().Spawn();
        }
    }

    public Vector3 GetPlayerSpawnPosition(int playerIndex)
    {
        // Make sure to handle the case where there are fewer spawn points than players
        return playerSpawnPositions[playerIndex % playerSpawnPositions.Count].position;
    }
}
