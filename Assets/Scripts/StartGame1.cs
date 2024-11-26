using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame1 : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LobbyTutorial_Done");
    }
}
