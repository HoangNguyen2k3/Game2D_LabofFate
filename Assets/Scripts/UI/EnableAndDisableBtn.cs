using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAndDisableBtn : MonoBehaviour
{
    [SerializeField] private GameObject boxIntroduce;
    private bool isActive = false; 
    private void OnEnable()
    {
        boxIntroduce.SetActive(false);
    }
    public void ActiveBox()
    {
        if(isActive)
        {
            boxIntroduce.SetActive(false);
            isActive = false;
        }
        else
        {
            boxIntroduce.SetActive(true);
            isActive = true;
        }
    }
    public void ActiveBoxAll()
    {
        boxIntroduce.SetActive(true);
    }
}
