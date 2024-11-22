using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public bool isActive = false;
    public bool isAuto = true;
    public AimProjectile missile;
    private AimProjectile spawnedMissle;

    public int maxMissilePerSpray = 6;
    private int missileCount = 0;

    private bool canShoot = true;
    [SerializeField] private float firingRate = 0.5f;
    [SerializeField] private float cooldown = 2f;
    private float fireTimer = 0f;

    void Update()
    {
        if (!isActive || !isAuto) return;
        fireTimer += Time.deltaTime;
        
        if(missileCount >= maxMissilePerSpray)
        {
            missileCount = 0;
            canShoot = false;
            StartCoroutine(ShootCooldown());
        }

        if(canShoot && fireTimer >= firingRate) 
        {
            Shoot();
            missileCount += 1;
            fireTimer = 0;
        }
    }

    private void Shoot() {
        if(missile)
        {  
            spawnedMissle = Instantiate(missile, transform.position, Quaternion.identity);
            spawnedMissle.GetComponent<AimProjectile>().initialVelocity = Random.insideUnitCircle.normalized;
            spawnedMissle.GetComponent<AimProjectile>().initialSpeed = Random.Range(20f, 30f);
        } 
        
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

}
