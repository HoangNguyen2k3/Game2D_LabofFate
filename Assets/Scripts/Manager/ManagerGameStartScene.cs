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

    private bool isTeleported = false;
    private bool isWinTriggered = false;
    private float timer;

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

        timer += Time.deltaTime;

        // Check for enemies and players
        bool hasEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;
        bool hasPlayers = GameObject.FindGameObjectsWithTag("Player").Length > 0;

        // Handle boss defeat and teleport logic
        if (timer > 30)
        {
            Boss boss1 = FindObjectOfType<Boss>();
            Boss2 boss2 = FindObjectOfType<Boss2>();

            if (boss1 == null && !isWinTriggered)
            {
                TriggerWinCondition();
            }

            if (boss2 == null && !isTeleported)
            {
                TeleportPlayers(new Vector3(-310, -17, 0));
                ResetTimerOnServerRpc();
            }
        }

        if (!hasEnemies && !isWinTriggered)
        {
            TriggerWinCondition();
        }
        else if (!hasPlayers)
        {
            TriggerLoseCondition();
        }
    }

    private void TriggerWinCondition()
    {
        isWinTriggered = true;
        ActivateWinScreenClientRpc();
    }

    private void TriggerLoseCondition()
    {
        ActivateLoseScreenClientRpc();
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

    [ServerRpc(RequireOwnership = false)]
    private void ResetTimerOnServerRpc()
    {
        LevelTimer.Instance?.ResetTimer();
    }

    private void TeleportPlayers(Vector3 newPosition)
    {
        TeleportPlayersClientRpc(newPosition);
        isTeleported = true;
    }

    [ClientRpc]
    private void TeleportPlayersClientRpc(Vector3 newPosition)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.transform.position = newPosition;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        if (IsServer)
        {
            SceneManager.LoadScene("LobbyTutorial_Done");
        }
    }
}
