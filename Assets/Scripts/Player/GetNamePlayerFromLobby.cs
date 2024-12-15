using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GetNamePlayerFromLobby : MonoBehaviour
{
    public static GetNamePlayerFromLobby Instance { get; private set; }
    public string player_current_name = "HOANG";
    public bool isEnd = false;
    private void Update()
    {
        if (EditPlayerName.Instance&&isEnd==false)
        {
            if (EditPlayerName.Instance.GetPlayerName() != player_current_name)
            {
                player_current_name = EditPlayerName.Instance.GetPlayerName();
            }
        }
    }
}
