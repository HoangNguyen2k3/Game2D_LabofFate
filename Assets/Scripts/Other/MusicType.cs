using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicType : MonoBehaviour
{
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;
    [SerializeField] private AudioClip music3;
    private TMP_Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void ChangeTypeMusic()
    {
        Debug.Log(dropdown.value);
        if (BackgroundMusic.instance != null)
        {
            switch (dropdown.value)
            {
                case 0:
                    Debug.Log("hello word 3");
                    BackgroundMusic.instance.ChangeMusic(music1);
                    break;
                case 1:
                    Debug.Log("hello word 4");
                    BackgroundMusic.instance.ChangeMusic(music2);
                    break;
                case 2:
                    Debug.Log("hello word 5");
                    BackgroundMusic.instance.ChangeMusic(music3);
                    break;
                default:
                    Debug.LogWarning("Invalid sound type selected.");
                    break;
            }
        }
        else
        {
            Debug.LogError("BackgroundMusic instance is null.");
        }
    }
    void Update()
    {

    }
}
