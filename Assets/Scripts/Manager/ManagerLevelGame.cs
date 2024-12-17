using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManagerLevelGame : NetworkBehaviour
{
    private bool isTeleported1 = false;
    private bool isTeleported2 = false;
    private bool isWinTriggered = false;
    [SerializeField] private ManagerGameStartScene gameStartScene;
    public NetworkVariable<char> current_map_element = new NetworkVariable<char>('t', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        if(!FindObjectOfType<ManagerGameStartScene>())
        gameStartScene = FindObjectOfType<ManagerGameStartScene>();
        current_map_element.Value = 'f';

    }
    private void Update()
    {
        if (!IsServer) return;
        if (gameStartScene == null)
        {
            gameStartScene = FindObjectOfType<ManagerGameStartScene>();
        }

        if (gameStartScene)
        {
            Boss boss1 = FindObjectOfType<Boss>();
            Boss2 boss2 = FindObjectOfType<Boss2>();
            Boss3 boss3=FindObjectOfType<Boss3>();
            if (boss1 == null && !isTeleported1)
            {
                TeleportPlayers(new Vector3(210, -110, 0));
                gameStartScene.ResetTimerOnServerRpc();
                current_map_element.Value = 'i';
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                if (IsServer)
                {
                    for (int i = 0; i < player.Length; i++)
                    {
                        player[i].GetComponent<SlashManagerCombo>().currentElement.Value = SlashManagerCombo.Element.Ice;
                    }      
                }
            }
            if(boss2 == null && !isTeleported2 &&isTeleported1)
            {
                TeleportPlayerFinal(new Vector3(-118,5,0));
                gameStartScene.ResetTimerSecondOnServerRpc();
                current_map_element.Value = 't';
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                if (IsServer)
                {
                    for (int i = 0; i < player.Length; i++)
                    {
                        player[i].GetComponent<SlashManagerCombo>().currentElement.Value = SlashManagerCombo.Element.Lighting;
                    }
                }

            }
            if (boss3 == null && !isWinTriggered)
            {
                gameStartScene.TriggerWinCondition();
            }
        }
    }
    public void TeleportPlayers(Vector3 newPosition)
    {
        TeleportPlayersClientRpc(newPosition);
        isTeleported1 = true;
    }

    public void TeleportPlayerFinal(Vector3 newPosition)
    {
        TeleportPlayersClientRpc(newPosition);
        isTeleported2 = true;
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
    /*    [ClientRpc]
        public void Change_map_element(string name)
        {
            current_map_element.Value = name;
        }*/

}
