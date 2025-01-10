using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManagePuzzleRoom : NetworkBehaviour
{
    public Transform returnPlayer;
    
    public static ManagePuzzleRoom Instance { get; private set; }
    public int num_crystal = 4;
    public NetworkVariable<int> current_crystal = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public bool PlayerInRange;

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerInRange == false && collision.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PlayerInRange == true && collision.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}
