using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBegin : MonoBehaviour
{
    [SerializeField] private PortalManager portalManager1;
    [SerializeField] private PortalManager portalManager2;
    [SerializeField] private GameObject door_1;
    [SerializeField] private GameObject door_2;
    [SerializeField] private GameObject door_3;
    [SerializeField] private GameObject door_4;
    private bool openPuzzle = false;
    bool isDone = false;
    private void Start()
    {
        door_1.GetComponent<Door>().isOpen.Value = true;
        door_2.GetComponent<Door>().isOpen.Value = true;
        door_3.GetComponent<Door>().isOpen.Value = true;
        door_4.GetComponent<Door>().isOpen.Value = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && openPuzzle == false)
        {
            portalManager1.isActive = true;
            portalManager2.isActive = true;
            door_1.GetComponent<Door>().isClose.Value = true;
            door_2.GetComponent<Door>().isClose.Value = true;
            door_3.GetComponent<Door>().isClose.Value = true;
            door_4.GetComponent<Door>().isClose.Value = true;
            openPuzzle = true;
        }
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
      //  if (isDone) { return; }
/*        if (!collision.GetComponent<FireInPuzzle>())
        {
            Destroy(GameObject.Find("Portal_Red(Clone)"));
            Destroy(GameObject.Find("PortalPurple(Clone)"));
            door_1.GetComponent<Door>().isOpen.Value = true;
            door_2.GetComponent<Door>().isOpen.Value = true;
            door_3.GetComponent<Door>().isOpen.Value = true;
            door_4.GetComponent<Door>().isOpen.Value = true;
        }*/
    }
    private void Update()
    {
        if (portalManager1 == null)
        {
            portalManager1 = GameObject.Find("Portal_Red(Clone)").GetComponent<PortalManager>();
        }
        if (portalManager2 == null)
        {
            portalManager2 = GameObject.Find("PortalPurple(Clone)").GetComponent<PortalManager>();
        }
        
    }
}
