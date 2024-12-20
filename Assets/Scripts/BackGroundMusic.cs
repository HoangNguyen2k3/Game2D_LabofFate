using UnityEngine;
using Unity.Netcode;

public class BackgroundMusic : NetworkBehaviour
{
    public static BackgroundMusic instance;
    public float num_Sound = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
            DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            Destroy(gameObject); 
        }
    }
}
