using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapEnterPortalPuzzle : NetworkBehaviour
{
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private GameObject door_1;
    [SerializeField] private GameObject door_2;
    [SerializeField] private string name_portal;
    [SerializeField] private GameObject fire;
    private bool openPuzzle = false;
    private bool donePuzzle = false;
    private bool begin_portal=false;
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
    private void Update()
    {
        if (portalManager == null && !begin_portal)
        {
            portalManager = GameObject.Find(name_portal).GetComponent<PortalManager>();
            begin_portal = true;
        }
        if (portalManager && fire == null && !donePuzzle)
        {
            donePuzzle = true;
            portalManager.gameObject.GetComponent<NetworkObject>().Despawn();
            door_1.GetComponent<Door>().isOpen.Value = true;
            door_2.GetComponent<Door>().isOpen.Value = true;
        }
    }
}
