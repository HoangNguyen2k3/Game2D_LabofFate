using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SingletonNetwork<T> : NetworkBehaviour where T : SingletonNetwork<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
