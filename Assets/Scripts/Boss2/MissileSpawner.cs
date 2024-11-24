using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public bool isActive = false;
    public bool isAuto = true;
    public HomingMissile missile;
    private HomingMissile spawnedMissle;

    public int maxMissilePerSpray = 6;
    private int missileCount = 0;

    private bool canShoot = true;
    [SerializeField] private float firingRate = 0.5f;
    [SerializeField] private float cooldown = 2f;
    private float fireTimer = 0f;

    [SerializeField] private float missileInitSpeed = 50f;
    [SerializeField] private float missileMinSpeed = 5f;
    [SerializeField] private float missileSpeedChange = 5f;
    [SerializeField] private float missileDegChange = 180f;



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

    public void Shoot() {
        if(missile)
        {  
            spawnedMissle = Instantiate(missile, transform.position, Quaternion.identity);
            spawnedMissle.GetComponent<HomingMissile>().initialVelocity = new Vector2(Random.insideUnitCircle.x, Mathf.Abs(Random.insideUnitCircle.y)).normalized;
            spawnedMissle.GetComponent<HomingMissile>().initialSpeed = missileInitSpeed + Random.Range(-10,10);
            spawnedMissle.GetComponent<HomingMissile>().minSpeed = missileMinSpeed + Random.Range(-2,2);
            spawnedMissle.GetComponent<HomingMissile>().speedChange = missileSpeedChange;
            spawnedMissle.GetComponent<HomingMissile>().degChange = missileDegChange;
        }
        
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

}
