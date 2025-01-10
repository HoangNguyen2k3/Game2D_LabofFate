using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapBegin : NetworkBehaviour
{
    [SerializeField] private PortalManager portal_Red;
    [SerializeField] private PortalManager portal_Purple;
    [SerializeField] private GameObject door_1;
    [SerializeField] private GameObject door_2;
    [SerializeField] private GameObject door_3;
    [SerializeField] private List<Transform> list_change_red;
    [SerializeField] private List<Transform> list_change_purple;
    private GameObject fire_red;
    private GameObject fire_purple;
    private bool openPuzzle = false;
    bool isDone_red = false;
    bool isDone_Purple = false;
    private bool begin_portal_red = false;
    private bool begin_portal_purple = false;
    private bool done_map_begin = false;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    //    door_1.GetComponent<Door>().isOpen.Value = true;
     //   door_2.GetComponent<Door>().isOpen.Value = true;
     //   door_3.GetComponent<Door>().isOpen.Value = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && openPuzzle == false)
        {
            portal_Red.isActive = true;
            portal_Purple.isActive = true;
         //   door_1.GetComponent<Door>().isClose.Value = true;
         //   door_2.GetComponent<Door>().isClose.Value = true;
         //   door_3.GetComponent<Door>().isClose.Value = true;
         //   door_4.GetComponent<Door>().isClose.Value = true;
            openPuzzle = true;
        }     
    }
    private void Update()
    {
        if (portal_Red == null && !begin_portal_red && GameObject.Find("Portal_Red(Clone)"))
        {
        
            portal_Red = GameObject.Find("Portal_Red(Clone)").GetComponent<PortalManager>();
            fire_red = GameObject.Find("Fire_Red(Clone)");
            fire_red.GetComponent<FireInPuzzle>().trans = list_change_red;
            begin_portal_red = true;
        }
        if (portal_Purple == null && !begin_portal_purple && GameObject.Find("PortalPurple(Clone)"))
        {
            portal_Purple = GameObject.Find("PortalPurple(Clone)").GetComponent<PortalManager>();
            fire_purple = GameObject.Find("Fire_Purple(Clone)");
            fire_purple.GetComponent<FireInPuzzle>().trans=list_change_purple;
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
            //   door_1.GetComponent<Door>().isOpen.Value = true;
            //   door_2.GetComponent<Door>().isOpen.Value = true;
            //  door_3.GetComponent<Door>().isOpen.Value = true;
            DonDestroyClientRpc();
            //    door_4.GetComponent<Door>().isOpen.Value = true;
            done_map_begin = true;
        }
    }
    [ClientRpc]
    public void DonDestroyClientRpc()
    {
        door_1.GetComponent<DestroyGameObjectInAnimation>().DoneDestroy();
        door_2.GetComponent<DestroyGameObjectInAnimation>().DoneDestroy();
        door_3.GetComponent<DestroyGameObjectInAnimation>().DoneDestroy();
    }
}
