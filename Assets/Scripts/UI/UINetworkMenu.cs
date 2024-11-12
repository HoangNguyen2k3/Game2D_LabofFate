using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UINetworkMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldNamePlayer;
    [SerializeField] private ManagerGameStartScene managerGameStartScene; // Reference to ManagerGameStartScene
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject UIPLayer;

    private void Start()
    {
        map.SetActive(false);
        UIPLayer.SetActive(false);
    }
    public void StartHost()
    {
        SettingInputName();
        NetworkManager.Singleton.StartHost();

        // Spawn enemies when hosting
        managerGameStartScene.SpawnEnemies();
        map.SetActive(true);
        UIPLayer.SetActive(true);

        gameObject.SetActive(false);
    }

    public void StartClient()
    {
        SettingInputName();
        NetworkManager.Singleton.StartClient();
        map.SetActive(true);
        UIPLayer.SetActive(true);
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
