using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal_center : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform beginPosition;
    [SerializeField] private float speed = 3f;

    private bool isMoving = false;
    private bool isReturning = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                if (ManagePuzzleRoom.Instance.current_crystal == 3)
                {
                    ManagePuzzleRoom.Instance.current_crystal++;
                    Destroy(gameObject);
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
}
