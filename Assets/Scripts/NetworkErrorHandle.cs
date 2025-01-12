using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkErrorHandler : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
          //  gameObject.SetActive(true);
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        if (GameObject.Find("NetworkManager"))
        {
            Destroy(GameObject.Find("NetworkManager"));
        }
        SceneManager.LoadScene("UpdatedLobbyTutorial_Done");
    }
}
