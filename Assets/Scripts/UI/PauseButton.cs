using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton :MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuWindow;
    [SerializeField] private GameObject openSound;
    [SerializeField] private GameObject closeSound;
/*
    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;
    [SerializeField] private GameObject map3;*/

   // [SerializeField] private GameObject manager_level;
 //   private bool change_first = false;
  //  private bool change_last = false;
    private void Start()
    {
        PauseMenuWindow.SetActive(false);
     //   DontDestroyOnLoad(gameObject);
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

        PauseMenuWindow.SetActive(false);
    }
    private void Update()
    {
      /*  if (manager_level.gameObject == null&& FindFirstObjectByType<ManagerLevelGame>())
        {
            manager_level = FindFirstObjectByType<ManagerLevelGame>().gameObject;
        }
        else
        {
            if (manager_level && manager_level.GetComponent<ManagerLevelGame>().current_map == 2 && !change_first)
            {
                change_first = true;
                map1.SetActive(false);
                map2.SetActive(true);
            }
            if (manager_level && manager_level.GetComponent<ManagerLevelGame>().current_map == 3 && !change_last)
            {
                change_last = true;
                map2.SetActive(false);
                map3.SetActive(true);
            }
        }*/

    }
}
