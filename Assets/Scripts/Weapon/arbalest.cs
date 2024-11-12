using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class Arbalest : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform positionSpawn;
    [SerializeField] private float timeDelayFireArbalest = 1f;
    private bool canAttack = true;

    private void Update()
    {
        if (!IsOwner) return; // Ch? ng??i s? h?u ??i t??ng m?i có th? b?n

        if (canAttack && Input.GetMouseButtonDown(0))
        {
            FireBulletServerRpc(); // G?i yêu c?u b?n ??n lên server
            canAttack = false;
            StartCoroutine(DelayFire());
        }
    }

    [ServerRpc]
    private void FireBulletServerRpc()
    {
        // T?o ??n và ??ng b? trên t?t c? các máy khách
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<NetworkObject>().Spawn();
    }

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(timeDelayFireArbalest);
        canAttack = true;
    }
}
