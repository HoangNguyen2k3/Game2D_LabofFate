using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerGameStartScene : NetworkBehaviour
{
    public static string PlayerName { get; set; }

    public string player_names = "";
    private bool only_onetime = true;

    [SerializeField] private List<Transform> playerSpawnPositions;
    [Header("1st Floor")]
    [SerializeField] private List<GameObject> typeEnemySpawn;
    [SerializeField] private List<Transform> positionSpawn;
    [Header("2nd Floor")]
    [SerializeField] private List<GameObject> typeEnemySpawn_map2;
    [SerializeField] private List<Transform> positionSpawn_map2;
    [Header("3rd Floor")]
    [SerializeField] private List<GameObject> typeEnemySpawn_map3;
    [SerializeField] private List<Transform> positionSpawn_map3;


    [SerializeField] private GameObject winGame;
    [SerializeField] private GameObject loseGame;



    private bool isWinTriggered = false;
    public float timer;

    private void Start()
    {
        if (!IsServer) { gameObject.SetActive(false); }
        winGame.SetActive(false);
        loseGame.SetActive(false);
       
/*        if (IsServer)
        {
            SpawnEnemiesServerRpc();
        }*/
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
           // string play
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            foreach (var item in player)
            {
                int temp = item.GetComponent<PlayerSetting>().playerName.text.Length;
                if ( temp> 5) {
                    player_names += item.GetComponent<PlayerSetting>().playerName.text.Substring(0,5) + "-";
                }
                else if(temp <= 5&&temp>=1)
                {
                    player_names += item.GetComponent<PlayerSetting>().playerName.text + "-";
                }
                else
                {
                    player_names += "Anony";
                }
            }
            player_names= player_names.Substring(0,player_names.Length-1);
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
    [ServerRpc]
    public void SpawnEnemiesMap2ServerRpc()
    {
        for (int i = 0; i < positionSpawn_map2.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn_map2[i], positionSpawn_map2[i].position, Quaternion.identity);
            if (spawnEnemy.GetComponent<NetworkObject>())
            {
                spawnEnemy.GetComponent<NetworkObject>().Spawn();
            }

        }
    }
    [ServerRpc]
    public void SpawnEnemiesMap3ServerRpc()
    {
        for (int i = 0; i < positionSpawn_map3.Count; i++)
        {
            GameObject spawnEnemy = Instantiate(typeEnemySpawn_map3[i], positionSpawn_map3[i].position, Quaternion.identity);
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
        if (only_onetime)
        {
            only_onetime = false;
            int temp = (int)LevelTimer.Instance.total_time.Value;
            ScoreManager.instance.SubmitScore(player_names, (1800 - temp));
        }

        StartCoroutine(waitToEnd());
        
    }
    public IEnumerator waitToEnd()
    {
        yield return new WaitForSeconds(2f);
        ResetLeaderBoardClientRpc();
        ActivateWinScreenClientRpc();
    }
    [ClientRpc]
    private void ResetLeaderBoardClientRpc()
    {
        if (!IsServer)
        {
            LeaderBoard.instance.GetLeaderboard();
        }

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
    public void ReturnToMenuSecond()
    {
        if (GameObject.Find("NetworkManager"))
        {
            Destroy(GameObject.Find("NetworkManager"));
        }
        SceneManager.LoadScene("UpdatedLobbyTutorial_Done");
    }
}
