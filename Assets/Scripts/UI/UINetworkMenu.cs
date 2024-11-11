using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UINetworkMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldNamePlayer;
    [SerializeField] private ManagerGameStartScene managerGameStartScene; // Reference to ManagerGameStartScene

    public void StartHost()
    {
        SettingInputName();
        NetworkManager.Singleton.StartHost();

        // Spawn enemies when hosting
        managerGameStartScene.SpawnEnemies();

        gameObject.SetActive(false);
    }

    public void StartClient()
    {
        SettingInputName();
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SettingInputName()
    {
        // Store player name in ManagerGameStartScene's static variable
        ManagerGameStartScene.PlayerName = inputFieldNamePlayer.text;
    }
}
