using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePuzzleRoom : MonoBehaviour
{
    public Transform returnPlayer;
    
    public static ManagePuzzleRoom Instance { get; private set; }
    public int num_crystal = 4;
    public int current_crystal = 0;
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
