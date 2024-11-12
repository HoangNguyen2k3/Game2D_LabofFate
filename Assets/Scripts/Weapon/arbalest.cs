using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arbalest : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform positionSpawn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(bullet, positionSpawn.position, Quaternion.identity);
        }
    }
}
