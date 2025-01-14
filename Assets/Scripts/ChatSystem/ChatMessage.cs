using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textChat;
    public void SetText(string text)
    {
        textChat.text=text;
    }
}
