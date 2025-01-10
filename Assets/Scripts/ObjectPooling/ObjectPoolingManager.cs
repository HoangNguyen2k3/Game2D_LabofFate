using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolingManager : NetworkBehaviour
{
    public int poolSize = 10;
    //public Queue<GameObject> pool;
    [SerializeField] private List<GameObject> poolList;
    public NetworkVariable<int> current_bullet = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    [SerializeField] private GameObject poolGameObject;
    [SerializeField] private GameObject bullet;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        if (bullet == null || poolGameObject == null)
        {
            return;
        }

        poolList.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newChild = Instantiate(bullet, transform.position, Quaternion.identity);
            var networkObject = newChild.GetComponent<NetworkObject>();
            networkObject.Spawn();
            newChild.transform.SetParent(poolGameObject.transform);
            poolList.Add(newChild);
            SetInactiveClientRpc(networkObject.NetworkObjectId);
        }
    }

    [ClientRpc]
    private void SetInactiveClientRpc(ulong objectId)
    {
        if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
        {
            networkObject.gameObject.SetActive(false);
        }
    }
    public GameObject GetBullet()
    {
        if (!IsServer) { return null; }

        GameObject obj;
        if (current_bullet.Value < poolSize)
        {
            obj = poolList[current_bullet.Value];
            current_bullet.Value++;
        }
        else
        {
            obj = poolList[0];
            current_bullet.Value = 1;
        }
        obj.SetActive(true);

        SetBulletActiveClientRpc(obj.GetComponent<NetworkObject>().NetworkObjectId, true);

        return obj;
    }
    [ClientRpc]
    private void SetBulletActiveClientRpc(ulong networkObjectId, bool isActive)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.SpawnedObjects[networkObjectId];
        if (netObj != null)
        {
            netObj.gameObject.SetActive(isActive);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (!IsServer) { return; }
        obj.SetActive(false);
        SetBulletActiveClientRpc(obj.GetComponent<NetworkObject>().NetworkObjectId, false);
    }
    /*   private void Awake()
       {
           pool = new Queue<GameObject>();
           for(int i = 0; i < poolSize; i++)
           {
               GameObject obj = Instantiate(bulletPrefab);
               obj.SetActive(false);
               pool.Enqueue(obj);
           }
       }
       public GameObject GetBullet()
       {
           if(pool.Count > 0)
           {
               GameObject obj = pool.Dequeue();
               obj.SetActive(true);
               return obj;
           }
           else
           {
               GameObject obj = Instantiate(bulletPrefab);
               return obj;
           }
       }
       public void ReturnObject(GameObject obj)
       {
           obj.SetActive(false);
           pool.Enqueue(obj);
       }*/
}
