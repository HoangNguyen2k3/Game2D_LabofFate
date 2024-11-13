using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Arbalest : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform positionSpawn;
    [SerializeField] private float timeDelayFireArbalest = 1f;
    private bool canAttack = true;

    private NetworkObject parentNetworkObject;

    private void Start()
    {
        // Get the parent NetworkObject (assumes the Arbalest is a child of the owning player or object)
        parentNetworkObject = transform.root.GetComponent<NetworkObject>();
    }

    private void Update()
    {
        // Check if the root NetworkObject is the owner
        if (parentNetworkObject == null || !parentNetworkObject.IsOwner) return;

        if (canAttack && Input.GetMouseButtonDown(0))
        {
            FireBullet(); // Fire bullet directly on the client side
            canAttack = false;
            StartCoroutine(DelayFire());
        }
    }

    private void FireBullet()
    {
        // Instantiate bullet locally and spawn across network
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<NetworkObject>().Spawn(true);
    }

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(timeDelayFireArbalest);
        canAttack = true;
    }
}
