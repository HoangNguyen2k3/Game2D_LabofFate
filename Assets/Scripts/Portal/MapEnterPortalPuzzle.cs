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
    private GameObject fire;
    [SerializeField] private List<Transform> list_change_trans;
    [SerializeField] private string name_fire;
    private bool openPuzzle = false;
    public bool donePuzzle = false;
    private bool begin_portal=false;
    private void Start()
    {


    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NotActiveClientRpc();
        //  door_1.GetComponent<Door>().isOpen.Value = true;
        //  door_2.GetComponent<Door>().isOpen.Value = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&openPuzzle==false)
        {
            portalManager.isActive = true;
            DoneActiveClientRpc();
            openPuzzle = true;
        }
    }
    private void Update()
    {
        if (portalManager == null &&fire==null&& !begin_portal)
        {
            portalManager = GameObject.Find(name_portal).GetComponent<PortalManager>();
            fire = GameObject.Find(name_fire);
            fire.GetComponent<FireInPuzzle>().trans=list_change_trans;
            begin_portal = true;
        }
        if (portalManager && fire == null && !donePuzzle)
        {
            donePuzzle = true;
            portalManager.gameObject.GetComponent<NetworkObject>().Despawn();
            //  door_1.GetComponent<Door>().isOpen.Value = true;
            //  door_2.GetComponent<Door>().isOpen.Value = true;
            NotActiveClientRpc();
        }
    }
    [ClientRpc]
    public void DoneActiveClientRpc()
    {
        door_1.GetComponent<DestroyGameObjectInAnimation>().Active_gameObject();
        door_2.GetComponent<DestroyGameObjectInAnimation>().Active_gameObject() ;
    }
    [ClientRpc]
    public void NotActiveClientRpc()
    {
        door_1.GetComponent<DestroyGameObjectInAnimation>().InActive_gameObject();
        door_2.GetComponent<DestroyGameObjectInAnimation>().InActive_gameObject();
    }
}
