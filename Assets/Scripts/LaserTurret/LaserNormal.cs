using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserNormal : MonoBehaviour
{
    [SerializeField] private GameObject bloom;
    void Start()
    {
    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(bloom, collision.transform.position, Quaternion.identity);
        }
    }
}
