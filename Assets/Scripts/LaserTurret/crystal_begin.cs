using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal_begin : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    private bool isMoving=false;
    [SerializeField] private float speed=5f;
    // Start is called before the first frame update
    void Start()
    {
       // targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (ManagePuzzleRoom.Instance.current_crystal == 0&&isMoving)
        {
            transform.position= Vector2.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, targetPosition.position) < 0.01f)
        {
            ManagePuzzleRoom.Instance.current_crystal ++;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = true;
        }
    }
}
