using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : MonoBehaviour
{
    public float cutsceneTime;
    
    private void Update()
    {
        cutsceneTime -= Time.deltaTime;
        if (cutsceneTime <= 0 || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LobbyTutorial_Done");
        }
    }
}
