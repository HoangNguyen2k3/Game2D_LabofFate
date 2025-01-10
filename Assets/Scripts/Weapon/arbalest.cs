using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Arbalest : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletClient;
    [SerializeField] private Transform positionSpawn;
    [SerializeField] private float timeDelayFireArbalest = 1f;
    public bool canAttack = true;

    private void Start()
    {

    }


    private void Update()
    {
        if (!IsOwner) return;

        if (canAttack && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            Vector3 direction = (mousePos - positionSpawn.position).normalized;
            if (!IsHost)
            {
                FireBullet(direction);
            }
            
            FireBulletServerRpc(direction, OwnerClientId);
            canAttack = false;
            StartCoroutine(DelayFire());
        }
    }

    [ServerRpc]
    private void FireBulletServerRpc(Vector3 direction, ulong shooterClientId)
    {
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<ProjectilePlayer>().Initialize(direction);
        NetworkObject networkObject = bulletInstance.GetComponent<NetworkObject>();
        networkObject.Spawn(true);
        HideBulletClientRpc(networkObject.NetworkObjectId, shooterClientId);
    }

    [ClientRpc]
    private void HideBulletClientRpc(ulong bulletNetworkObjectId, ulong shooterClientId)
    {
        if (NetworkManager.Singleton.LocalClientId == shooterClientId&&!IsHost)
        {
            NetworkObject bulletObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[bulletNetworkObjectId];
            if (bulletObject != null)
            {
                bulletObject.gameObject.SetActive(false);
            }
        }
    }

    private void FireBullet(Vector3 direction)
    {
        GameObject bulletTemp = Instantiate(bulletClient, positionSpawn.position, Quaternion.identity);
        bulletTemp.GetComponent<BulletClient>().Initialize(direction);
    }
    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(timeDelayFireArbalest);
        canAttack = true;
    }
    private void OnDisable()
    {
        canAttack = true;
    }
}
