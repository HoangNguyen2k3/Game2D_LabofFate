using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ManagerGameStartScene : MonoBehaviour
{
    public static string PlayerName { get; set; } // Static player name accessible from other scripts

    [SerializeField] private List<Transform> positionSpawn;
    [SerializeField] private List<GameObject> typeEnemySpawn;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            StatusLabels();
        GUILayout.EndArea();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < positionSpawn.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn[i], positionSpawn[i].position, Quaternion.identity);
            spawnEnemy.GetComponent<NetworkObject>().Spawn();
        }
    }
}
