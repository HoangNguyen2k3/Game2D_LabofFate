using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : MonoBehaviour
{
    public Transform target;
    [SerializeField] private GameObject bloom;
    [SerializeField] private GameObject laserNormal;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        target = UpdateTargetPlayer();
        Vector3 direction = (-target.position + transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        int temp = Random.Range(4, 12);
        for(int i = 1; i <= temp; i++)
        {
            Instantiate(laserNormal, transform.position, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg-(360/temp)*i));
        }
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
    private Transform UpdateTargetPlayer()
    {
        if (target != null)
        {
            Transform newTranform = target;
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
            {
                if (Vector2.Distance(transform.position, player[i].transform.position) <
                    Vector2.Distance(transform.position, newTranform.position))
                {
                    newTranform = player[i].transform;
                }
            }
            return newTranform;
        }
        return target;
    }
}
