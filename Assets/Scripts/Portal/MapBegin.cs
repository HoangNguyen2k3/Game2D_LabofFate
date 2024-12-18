using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapBegin : MonoBehaviour
{
    [SerializeField] private PortalManager portal_Red;
    [SerializeField] private PortalManager portal_Purple;
    [SerializeField] private GameObject door_1;
    [SerializeField] private GameObject door_2;
    [SerializeField] private GameObject door_3;
  //  [SerializeField] private GameObject door_4;
    [SerializeField] private GameObject fire_red;
    [SerializeField] private GameObject fire_purple;
    private bool openPuzzle = false;
    bool isDone_red = false;
    bool isDone_Purple = false;
    private bool begin_portal_red = false;
    private bool begin_portal_purple = false;
    private bool done_map_begin = false;
    private void Start()
    {
        door_1.GetComponent<Door>().isOpen.Value = true;
        door_2.GetComponent<Door>().isOpen.Value = true;
        door_3.GetComponent<Door>().isOpen.Value = true;
   //     door_4.GetComponent<Door>().isOpen.Value = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && openPuzzle == false)
        {
            portal_Red.isActive = true;
            portal_Purple.isActive = true;
            door_1.GetComponent<Door>().isClose.Value = true;
            door_2.GetComponent<Door>().isClose.Value = true;
            door_3.GetComponent<Door>().isClose.Value = true;
         //   door_4.GetComponent<Door>().isClose.Value = true;
            openPuzzle = true;
        }     
    }
    private void Update()
    {
        if (portal_Red == null && !begin_portal_red)
        {
        
            portal_Red = GameObject.Find("Portal_Red(Clone)").GetComponent<PortalManager>();
            begin_portal_red = true;
        }
        if (portal_Purple == null && !begin_portal_purple)
        {
            portal_Purple = GameObject.Find("PortalPurple(Clone)").GetComponent<PortalManager>();
            begin_portal_purple = true;
        }
        if (portal_Red)
        {
            if (fire_red == null && isDone_red==false)
            {
                portal_Red.GetComponent<NetworkObject>().Despawn();
                isDone_red = true;
            }
        }
        if (portal_Purple)
        {
            if (fire_purple == null && isDone_Purple==false)
            {
                portal_Purple.GetComponent<NetworkObject>().Despawn();
                isDone_Purple = true;
            }
        }
        if (isDone_red && isDone_Purple && !done_map_begin)
        {
            door_1.GetComponent<Door>().isOpen.Value = true;
            door_2.GetComponent<Door>().isOpen.Value = true;
            door_3.GetComponent<Door>().isOpen.Value = true;
        //    door_4.GetComponent<Door>().isOpen.Value = true;
            done_map_begin = true;
        }
    }
}
