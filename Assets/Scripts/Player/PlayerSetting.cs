using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetting : NetworkBehaviour
{
    public TextMeshPro playerName;
    public NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("HOANG",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();

    public static PlayerSetting Instance;
    private PlayerHealth health;
    public bool isFirstTimeEnterMap = false;
    [SerializeField] private GameObject playerUI;

    [SerializeField]
    private MinimapController minimapController;
    private void Start()
    {
        Instance = this;
        health = GetComponent<PlayerHealth>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            minimapController.gameObject.SetActive(true); 
        }
        else
        {
            minimapController.gameObject.SetActive(false); 
        }

        if (IsOwner)
        {
            playerUI.SetActive(false);
            networkPlayerName.Value = "HOANG";
            if (EditPlayerName.Instance)
            { 
                networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();
              //  playerName.text = networkPlayerName.Value.ToString();
            }
            else
            {
                if (FindFirstObjectByType<GetNamePlayerFromLobby>() != null)
                {
                    GameObject test = FindFirstObjectByType<GetNamePlayerFromLobby>().gameObject;
                    networkPlayerName.Value = test.GetComponent<GetNamePlayerFromLobby>().player_current_name;
                   // playerName.text = networkPlayerName.Value.ToString();
                    test.GetComponent<GetNamePlayerFromLobby>().isEnd = true;
                }
            }

            /*            if (IsHost)
                        {
                            Vector3 spawnPosition =new Vector3(-300, -10, 0);
                            SetPlayerPositionServerRpc(spawnPosition);
                        }
                        else
                        {
                            Vector3 spawnPosition = FindObjectOfType<ManagerGameStartScene>().GetPlayerSpawnPosition((int)OwnerClientId);
                            SetPlayerPositionServerRpc(spawnPosition);
                        }*/
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            SetPlayerPositionServerRpc(spawnPosition);
            playerName.text = networkPlayerName.Value.ToString();
        }
        else
        {
            playerName.text = networkPlayerName.Value.ToString();
        }
        /*        else
                {
                    playerName.text = networkPlayerName.Value.ToString();
                    networkPlayerName.OnValueChanged += NetworkPlayerName_OnValueChanged;

                }*/
        
        playerPosition.OnValueChanged += OnPlayerPositionChanged;
        transform.position = playerPosition.Value;
    }

    private void OnPlayerPositionChanged(Vector3 previousValue, Vector3 newValue)
    {
        transform.position = newValue;
    }

    private void NetworkPlayerName_OnValueChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        playerName.text = newValue.Value;
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerPositionServerRpc(Vector3 newPosition)
    {
        playerPosition.Value = newPosition;
    }
    private void Update()
    {
        if (GameObject.Find("MainGame") != null && !isFirstTimeEnterMap && IsOwner)
        {
            playerUI.SetActive(true);
        }
        if (GameObject.Find("MainGame")!=null&&!isFirstTimeEnterMap&&IsServer)
        {
            isFirstTimeEnterMap = true;
            Vector3 spawnPosition = new Vector3(0, -7, 0);
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in player)
            {
                p.GetComponent<PlayerController>().stopMovingInstruction = false;
            }
            SetPlayerPositionServerRpc(spawnPosition);
            health.currentHealth.Value = 30;
        }
    }
}
