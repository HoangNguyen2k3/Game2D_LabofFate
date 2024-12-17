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

    }


    private void Update()
    {
        if (!IsOwner) return;

        if (canAttack && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            Vector3 direction = (mousePos - positionSpawn.position).normalized;

            FireBulletServerRpc(direction);
            canAttack = false;
            StartCoroutine(DelayFire());
        }
    }

    [ServerRpc]
    private void FireBulletServerRpc(Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(bullet, positionSpawn.position, Quaternion.identity);

        bulletInstance.GetComponent<ProjectilePlayer>().Initialize(direction);

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
