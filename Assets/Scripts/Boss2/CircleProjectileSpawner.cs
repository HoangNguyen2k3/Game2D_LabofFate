using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CircleBulletSpawner : MonoBehaviour
{
    public bool isActive = false;
    public bool isAuto = true;
    public int numberOfProjectile = 8;
    public float rotateDeg = 10;
    public Projectile projectile;
    private Projectile spawnedProjectile;
    private Vector2 velocity = Vector2.right;

    public int numberOfBurst = 4;
    private int burstCounter = 0;
    public float burstCooldown = 2;
    private float burstTimer = 0f;
    private bool canShoot = true;
    public float shootCooldown = 2f;

    void Update()
    {
        if (!isActive || !isAuto) 
        {
            burstCounter = 0;
            burstTimer = 0;
            return;
        }
        
        if (burstCounter >= numberOfBurst)
        {
            if ((burstTimer += Time.deltaTime) > burstCooldown)
            {
                burstCounter = 0;
                burstTimer = 0;
            }
            else
            {
                return;
            }

        }

        if(canShoot) 
        {   
            velocity = rotate(velocity, rotateDeg);
            burstCounter++;
            Shoot();
            canShoot = false;
            StartCoroutine(ShootCooldown());
        }
    }

    public void Shoot() {
        if(projectile)
        {  
            float angle = 360f / numberOfProjectile;
            
            int numberOfSpawnedProjectile = 0;
            while (numberOfSpawnedProjectile < numberOfProjectile)
            {        
                spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                spawnedProjectile.GetComponent<Projectile>().transform.right = velocity;

                numberOfSpawnedProjectile++;
                velocity = rotate(velocity, angle);
            }
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private Vector2 rotate(Vector2 v, float delta) {
    delta *= Mathf.Deg2Rad;
    return new Vector2(
        v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
        v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
    );
}

}
