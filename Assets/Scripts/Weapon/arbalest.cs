using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Arbalest : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform positionSpawn;
    [SerializeField] private float timeDelayFireArbalest = 1f;
    public bool canAttack = true;

    private void Start()
    {
        // Get the parent NetworkObject (assumes the Arbalest is a child of the owning player or object)

    }

    private void Update()
    {
        // Check if the root NetworkObject is the owner
        if (!IsOwner) return;

        if (canAttack && Input.GetMouseButtonDown(0))
        {
            FireBulletServerRPC(); 
            canAttack = false;
            StartCoroutine(DelayFire());
        }
    }
    [ServerRpc]
    private void FireBulletServerRPC()
    {
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<NetworkObject>().Spawn(true);
      //  FireBulletClientRPC();
    }
    [ClientRpc]
    private void FireBulletClientRPC()
    {
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<NetworkObject>().Spawn(true);
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
