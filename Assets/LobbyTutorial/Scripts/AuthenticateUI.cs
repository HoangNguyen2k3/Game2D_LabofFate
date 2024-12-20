using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private GameObject settingWindow;


    private void Awake() {
       CloseSettingWindow();
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
        });
        settingButton.onClick.AddListener(() =>
        {
            OpenSettingWindow();
        });
        exitGameButton.onClick.AddListener(() =>
        {
            ExitGameButton();
        });
    }

    public void OpenSettingWindow()
    {
        settingWindow?.SetActive(true);
    }
    public void CloseSettingWindow()
    {
        settingWindow?.SetActive(false);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ExitGameButton()
    {
        Application.Quit();
    }

}