using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuWindow;
    [SerializeField] private GameObject openSound;
    [SerializeField] private GameObject closeSound;
    private void Start()
    {
        PauseMenuWindow.SetActive(false);
    }
    public void ContinueGame()
    {
        PauseMenuWindow?.SetActive(false);
    }
    public void PauseGame()
    {
        PauseMenuWindow?.SetActive(true);
    }
    public void OpenSound()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            AudioListener.volume = BackgroundMusic.instance.num_Sound;
        }
        openSound?.SetActive(true);
        closeSound?.SetActive(false);
    }
    public void UnSound()
    {
        AudioListener.volume = 0;
        openSound?.SetActive(false);
        closeSound?.SetActive(true);
    }
    public void ReturnToMenu()
    {
            if (GameObject.Find("NetworkManager"))
            {
                Destroy(GameObject.Find("NetworkManager"));
            }
            SceneManager.LoadScene("UpdatedLobbyTutorial_Done");

        
    }
}
