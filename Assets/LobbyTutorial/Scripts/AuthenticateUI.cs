using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitGameButton;


    private void Awake() {
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
        });
        exitGameButton.onClick.AddListener(() =>
        {
            ExitGameButton();
        });
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void ExitGameButton()
    {
        Application.Quit();
    }

}