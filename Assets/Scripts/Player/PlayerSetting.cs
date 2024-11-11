using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetting : NetworkBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    private NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("Unknown");

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Get player name from ManagerGameStartScene's static variable
            string playerNameInput = ManagerGameStartScene.PlayerName;
            SetPlayerNameServerRpc(playerNameInput);
        }

        playerName.text = networkPlayerName.Value.ToString();
        networkPlayerName.OnValueChanged += NetworkPlayerName_OnValueChanged;
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
}
