using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class UINetwork : MonoBehaviour
{
    public TMP_InputField inputNamePlayer;
    public string playerNameInput = "";
    [SerializeField] private List<Transform> positionSpawn;
    [SerializeField] private List<GameObject> typeEnemySpawn;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
                {
  StatusLabels();
                }
          

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        GUILayout.Label("Enter Player Name:");
        playerNameInput = GUILayout.TextField(playerNameInput, 25);
        if (inputNamePlayer != null)
        {
            inputNamePlayer.text = playerNameInput;
        }

        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
            SpawnEnemy();
/*            if (enemy != null)
            {
                GameObject spawnedEnemy = Instantiate(enemy);
                spawnedEnemy.GetComponent<NetworkObject>().Spawn();
            }*/

        }
        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
        }
        if (GUILayout.Button("Server"))
        {
            NetworkManager.Singleton.StartServer();
        }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
    private void SpawnEnemy()
    {
        for(int i = 0; i < positionSpawn.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn[i], positionSpawn[i].position,Quaternion.identity);
            spawnEnemy.GetComponent<NetworkObject>().Spawn();

        }
    }
}
