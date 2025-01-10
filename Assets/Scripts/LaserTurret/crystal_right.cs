using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class crystal_right : NetworkBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform beginPosition;
    [SerializeField] private float speed = 3f;

    private bool isMoving = false;
    private bool isReturning = false;

    void Update()
    {
        if (ManagePuzzleRoom.Instance.current_crystal.Value == 3)
        {
            Destroy(gameObject);
        }
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition.position) < 0.5f)
            {
                Debug.Log(ManagePuzzleRoom.Instance.current_crystal.Value);
                if (ManagePuzzleRoom.Instance.current_crystal.Value == 2)
                {
                    if (IsServer)
                    {
                        ManagePuzzleRoom.Instance.current_crystal.Value = 3;
                       
                    }
                   
                }
                else
                {
                    isMoving = false;
                    isReturning = true;
                }
            }
        }
        else if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, beginPosition.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, beginPosition.position) < 0.01f)
            {
                isReturning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isMoving && !isReturning)
        {
            isMoving = true;
        }
    }
    [ClientRpc]
    public void DestroyClientRpc()
    {
        Destroy(gameObject);
    }
}
