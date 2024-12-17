using UnityEngine;
using Unity.Netcode;

public class BackgroundMusic : NetworkBehaviour
{
    private static BackgroundMusic instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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
