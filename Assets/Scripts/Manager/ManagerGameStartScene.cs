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

    private bool isWinTriggered = false;
    public float timer;

    private void Start()
    {
        if(!IsServer)
        {
            Destroy(gameObject);
        }
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
            if (spawnEnemy.GetComponent<NetworkObject>())
            {
                spawnEnemy.GetComponent<NetworkObject>().Spawn();
            }
            
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

      //  timer += Time.deltaTime;

/*        if (timer > 0)
        {
            Boss boss1 = FindObjectOfType<Boss>();
            Boss2 boss2 = FindObjectOfType<Boss2>();
            if (boss1 == null && !isTeleported)
            {
                TeleportPlayers(new Vector3(210, -110, 0));
                ResetTimerOnServerRpc();
            }
            if (boss2 == null && !isWinTriggered)
            {
                TriggerWinCondition();
            }


        }*/

        if (!hasEnemies && !isWinTriggered)
        {
            TriggerWinCondition();
        }
        else if (!hasPlayers)
        {
            TriggerLoseCondition();
        }
    }

    public void TriggerWinCondition()
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
    public void ResetTimerOnServerRpc()
    {
        LevelTimer.Instance?.ResetTimerFirst();
    }
    [ServerRpc(RequireOwnership = false)]
    public void ResetTimerSecondOnServerRpc()
    {
        LevelTimer.Instance?.ResetTimerSecond();
    }
    /*    public void TeleportPlayers(Vector3 newPosition)
        {
            TeleportPlayersClientRpc(newPosition);
            isTeleported = true;
        }
    */


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
            if (GameObject.Find("NetworkManager"))
            {
                Destroy(GameObject.Find("NetworkManager"));
            }
            SceneManager.LoadScene("UpdatedLobbyTutorial_Done");

        }
    }
}
