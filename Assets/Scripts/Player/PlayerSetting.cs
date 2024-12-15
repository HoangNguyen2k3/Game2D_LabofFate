using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetting : NetworkBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    public NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("HOANG",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();

    public static PlayerSetting Instance;

    
    private void Start()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        networkPlayerName.Value = "HOANG";
        if (IsOwner)
        {
            if (EditPlayerName.Instance)
            { 
                networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();
                playerName.text = networkPlayerName.Value.ToString();
            }
            else
            {
                if (FindFirstObjectByType<GetNamePlayerFromLobby>() != null)
                {
                    GameObject test = FindFirstObjectByType<GetNamePlayerFromLobby>().gameObject;
                    networkPlayerName.Value = test.GetComponent<GetNamePlayerFromLobby>().player_current_name;
                    playerName.text = networkPlayerName.Value.ToString();
                    test.GetComponent<GetNamePlayerFromLobby>().isEnd = true;
                }
            }

            Debug.Log("KKK");
            if (IsHost)
            {
                Vector3 spawnPosition =new Vector3(-300, -10, 0);
                SetPlayerPositionServerRpc(spawnPosition);
            }
            else
            {
                Vector3 spawnPosition = FindObjectOfType<ManagerGameStartScene>().GetPlayerSpawnPosition((int)OwnerClientId);
                SetPlayerPositionServerRpc(spawnPosition);
            }   
        }
        playerName.text = networkPlayerName.Value.ToString();
        networkPlayerName.OnValueChanged += NetworkPlayerName_OnValueChanged;
        playerPosition.OnValueChanged += OnPlayerPositionChanged;
        // Set initial position
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


    [ServerRpc]
    private void SetPlayerPositionServerRpc(Vector3 newPosition)
    {
        playerPosition.Value = newPosition;
    }
    private void Update()
    {
        
    }
}
