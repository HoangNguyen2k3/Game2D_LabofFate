using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatButton : MonoBehaviour
{
    [SerializeField] private GameObject chatbox;
    public bool isActive = false;
    [SerializeField] private GameObject noticeIcon;
    private void OnEnable()
    {
        chatbox.SetActive(false);
    }
    public void ActiveBox()
    {
        if (isActive)
        {
            chatbox.SetActive(false);
            isActive = false;
        }
        else
        {
            noticeIcon.SetActive(false);
            chatbox.SetActive(true);
            isActive = true;
        }
    }
    public void ActiveBoxAll()
    {
        chatbox.SetActive(true);
    }
    private void OnDisable()
    {
        isActive = false;
    }
}
