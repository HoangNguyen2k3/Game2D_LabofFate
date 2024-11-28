using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomRangeManage : NetworkBehaviour
{
    private bool isOpenDoor = false;

    [SerializeField] private GameObject door;

    public List<GameObject> enemiesInRange = new List<GameObject>(); 

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject); 
        }
        if (enemiesInRange.Count == 0 && !isOpenDoor)
        {
                door.GetComponent<Door>().OpenDoorServerRpc();
                isOpenDoor = true;
           
        }
    }
}
