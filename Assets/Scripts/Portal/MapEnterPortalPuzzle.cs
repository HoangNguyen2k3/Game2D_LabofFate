using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapEnterPortalPuzzle : NetworkBehaviour
{
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private GameObject door_1;
    [SerializeField] private GameObject door_2;
    private bool openPuzzle = false;
    private void Start()
    {
        door_1.GetComponent<Door>().isOpen.Value = true;
        door_2.GetComponent<Door>().isOpen.Value = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&openPuzzle==false)
        {
            portalManager.isActive = true;
            door_1.GetComponent<Door>().isClose.Value = true;
            door_2.GetComponent<Door>().isClose.Value=true;
            openPuzzle = true;
        }
    }
}
