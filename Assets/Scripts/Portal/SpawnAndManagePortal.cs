using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAndManagePortal : NetworkBehaviour
{
   public List<Transform> list;
    public List<GameObject> portal_list;
    private void Start()
    {
        if (IsServer)
        {
            for(int i = 0; i < 5; i++)
            {
                GameObject portal = Instantiate(portal_list[i], list[i].position, Quaternion.identity);
                portal.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
