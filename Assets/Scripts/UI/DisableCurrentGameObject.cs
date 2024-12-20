using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCurrentGameObject : MonoBehaviour
{
    [SerializeField] private GameObject UIParent; 
    public void disableCurrentGameObject()
    {
        UIParent?.SetActive(false);
    }
}
