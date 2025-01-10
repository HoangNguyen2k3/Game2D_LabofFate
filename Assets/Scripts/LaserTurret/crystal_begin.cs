using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class crystal_begin : NetworkBehaviour
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
        if (ManagePuzzleRoom.Instance.current_crystal.Value == 0&&isMoving)
        {
            transform.position= Vector2.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, targetPosition.position) < 0.5f)
        {
            Debug.Log(ManagePuzzleRoom.Instance.current_crystal.Value);
            if (IsServer)
            {
                ManagePuzzleRoom.Instance.current_crystal.Value = 1;
             
            }
          Destroy(gameObject);

        }
        if (ManagePuzzleRoom.Instance.current_crystal.Value == 1)
        {
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
