using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private GameObject activeBow;
    [SerializeField] private GameObject activeSword;
    public bool usingSword = false;
    public bool usingArbalet = false;

    private void Start()
    {
        activeSword.SetActive(true);
        activeBow.SetActive(false);
        usingSword = true; 
        usingArbalet=false;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1)) {
            activeSword.SetActive(false);
            activeBow.SetActive(true);
            usingSword = false;
            usingArbalet = true;
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            activeSword.SetActive(true);
            activeBow.SetActive(false);               
            usingSword = true;
            usingArbalet = false;
        }

    }
}
