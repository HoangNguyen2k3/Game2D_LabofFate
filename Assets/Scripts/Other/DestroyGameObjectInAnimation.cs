using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroyGameObjectInAnimation : NetworkBehaviour
{
    public void DoneDestroy()
    {
        Destroy(gameObject);
    }
    public void InActive_gameObject()
    {
        gameObject.SetActive(false);
    }
    public void Active_gameObject() {
        gameObject.SetActive(true);
    }
    public void DonDestroyInNetwork()
    {
        if (gameObject.GetComponent<NetworkObject>())
        {
            if (IsServer)
            {
                gameObject.GetComponent<NetworkObject>().Despawn();
            }           
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
