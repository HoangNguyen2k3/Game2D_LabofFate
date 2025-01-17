using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager instance;

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private CanvasGroup chatContent;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private GameObject noticeIcon;
    [SerializeField] private ChatButton btn;

    public string playerName;

    public bool isSetPlayerName = false;

    private void Awake()
    {
        noticeIcon.SetActive(false);

    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
/*        if (IsOwner)
        {
            instance = this;
        }*/
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text,playerName);
            chatInput.text = "";
        }
        if (!isSetPlayerName && GameObject.Find("NetworkManager"))
        {
            isSetPlayerName = true;
            GameObject network = GameObject.Find("NetworkManager");
            playerName = network.GetComponent<GetNamePlayerFromLobby>().player_current_name;
        }
    }
    private void SendChatMessage(string content,string player_name)
    {
        if(string.IsNullOrWhiteSpace(content)) { return; }
        string S =player_name + " > " + content;
        SendChatMessageServerRpc(S);
    }
    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab,chatContent.transform);
        CM.SetText(msg);
    }
    [ServerRpc(RequireOwnership =false)]
    void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }
    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        if (btn.isActive==false)
        {
            noticeIcon?.SetActive(true);
        }
        else
        {
            noticeIcon?.SetActive(false);
        }
       
        AddMessage(message);
    }
    
}
