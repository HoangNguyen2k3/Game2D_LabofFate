using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ManagerGameStartScene : NetworkBehaviour
{
    public static string PlayerName { get; set; }

    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private List<GameObject> typeEnemySpawn;
    [SerializeField] private List<Transform> positionSpawn;
    [SerializeField] private GameObject winGame;
    [SerializeField] private GameObject loseGame;

    private void Start()
    {
        winGame.SetActive(false);
        loseGame.SetActive(false);
        if (IsServer)
        {
            SpawnEnemiesServerRpc();
        }

       
    }
    [ServerRpc]
    public void SpawnEnemiesServerRpc()
    {
        for (int i = 0; i < positionSpawn.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn[i], positionSpawn[i].position, Quaternion.identity);
            spawnEnemy.GetComponent<NetworkObject>().Spawn();
        }
    }

    public Vector3 GetPlayerSpawnPosition(int playerIndex)
    {
        return playerSpawnPositions[playerIndex % playerSpawnPositions.Count].position;
    }

    private void Update()
    {
        if (!IsServer) return;

        bool hasEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;
        bool hasPlayers = GameObject.FindGameObjectsWithTag("Player").Length > 0;

        if (!hasEnemies)
        {
            ActivateWinScreenClientRpc();
        }
        else if (!hasPlayers)
        {
            ActivateLoseScreenClientRpc();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    [ClientRpc]
    private void ActivateWinScreenClientRpc()
    {
        winGame.SetActive(true);
    }

    [ClientRpc]
    private void ActivateLoseScreenClientRpc()
    {
        loseGame.SetActive(true);
    }

    public void ReturnToMenu()
    {
        if (IsServer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}