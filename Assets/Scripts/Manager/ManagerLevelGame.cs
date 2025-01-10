using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerLevelGame : NetworkBehaviour
{
    private bool isTeleported1 = false;
    private bool isTeleported2 = false;
    private bool isWinTriggered = false;
    [SerializeField] private ManagerGameStartScene gameStartScene;
    public NetworkVariable<char> current_map_element = new NetworkVariable<char>('t', NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private bool boss2_exist = false;
    private bool boss3_exist = false;
    Boss2 boss2;
    Boss3 boss3;

    public int current_map = 1;
    /*    [SerializeField] private GameObject minimap_1;
        [SerializeField] private GameObject minimap_2;
        [SerializeField] private GameObject minimap_3;*/

    public override void OnNetworkSpawn()
    {
      //  if (!IsServer) { gameObject.SetActive(false); }
    }
    private void Start()
    {
        if (!IsServer) { return; }
        if (!FindObjectOfType<ManagerGameStartScene>())
        gameStartScene = FindObjectOfType<ManagerGameStartScene>();
        current_map_element.Value = 'f';
/*        minimap_1.SetActive(true);
        minimap_2.SetActive(false);
        minimap_3.SetActive(false);*/

    }
    private void Update()
    {
        if (!IsServer) { return; }
        if (gameStartScene == null)
        {
            gameStartScene = FindObjectOfType<ManagerGameStartScene>();
        }

        if (gameStartScene)
        {
            Boss boss1 = FindObjectOfType<Boss>();
         //   Boss2 boss2 = FindObjectOfType<Boss2>();
          //  Boss3 boss3 = FindObjectOfType<Boss3>();
            if (FindObjectOfType<Boss2>() && boss2_exist == false)
            {
                boss2_exist = true;
                boss2 = FindObjectOfType<Boss2>();
            }
            if (boss2_exist == true&&boss3_exist==false&& FindObjectOfType<Boss3>())
            {
                boss3 = FindObjectOfType<Boss3>();
                boss3_exist=true;
            }
            if (boss1 == null && !isTeleported1)
            {
                current_map = 2;
                TeleportPlayers(new Vector3(510, -100, 0));
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
            else if (boss2 == null && boss1 == null && !isTeleported2 &&isTeleported1&&boss2_exist==true)
            {
                current_map = 3;
                TeleportPlayerFinal(new Vector3(185, -25,0));
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
            else if (boss3 == null&&boss1==null&&boss2==null &&isTeleported1&&isTeleported2&& !isWinTriggered&&boss3_exist==true)
            {
                gameStartScene.TriggerWinCondition();
            }
        }
    }
    public void TeleportPlayers(Vector3 newPosition)
    {
        SyncPlayerPositionsClientRpc(newPosition);
        if(IsServer) { gameStartScene.SpawnEnemiesMap2ServerRpc(); }
       
        isTeleported1 = true;
    }

    public void TeleportPlayerFinal(Vector3 newPosition)
    {
        SyncPlayerPositionsClientRpc(newPosition);
       if (IsServer) { gameStartScene.SpawnEnemiesMap3ServerRpc(); }
        isTeleported2 = true;
    }
    [ClientRpc]
    private void SyncPlayerPositionsClientRpc(Vector3 newPosition)
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
