using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    // Start is called before the first frame update
    public bool change1 = true; 
    private GameObject Player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player== null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Player)
        {

        }
    }
    public void change()
    {
        if (Player) {
            if (change1)
            {
                Player.transform.position = new Vector3(210, -110, 0);
                change1 = false;            }
            else
            {
                Player.transform.position = new Vector3(-115, -35, 0);
            }
        }
    }
}
