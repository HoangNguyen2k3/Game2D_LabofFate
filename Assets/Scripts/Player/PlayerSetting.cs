using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetting : NetworkBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    public NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("HOANG");
    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();

    public static PlayerSetting Instance;
    private void Start()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (EditPlayerName.Instance)
            {
                networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();
            }
            
            Debug.Log("KKK");
            // Set the player's name
/*            string playerNameInput = ManagerGameStartScene.PlayerName;
            SetPlayerNameServerRpc(playerNameInput);*/
            if (IsHost)
            {
                Vector3 spawnPosition =new Vector3(-300, -10, 0);
                SetPlayerPositionServerRpc(spawnPosition);
            }
            else
            {
                Vector3 spawnPosition = GameObject.FindObjectOfType<ManagerGameStartScene>().GetPlayerSpawnPosition((int)OwnerClientId);
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
        // Update the position when it changes
        transform.position = newValue;
    }

    private void NetworkPlayerName_OnValueChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        playerName.text = newValue.Value;
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string newName)
    {
        networkPlayerName.Value = newName;
    }

    [ServerRpc]
    private void SetPlayerPositionServerRpc(Vector3 newPosition)
    {
        playerPosition.Value = newPosition;
    }
}
