using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetting : NetworkBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    private NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("Unknown");
    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Set the player's name
            string playerNameInput = ManagerGameStartScene.PlayerName;
            SetPlayerNameServerRpc(playerNameInput);

            // Set player position based on player index
            Vector3 spawnPosition = GameObject.FindObjectOfType<ManagerGameStartScene>().GetPlayerSpawnPosition((int)OwnerClientId);

            SetPlayerPositionServerRpc(spawnPosition);
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
