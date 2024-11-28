using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject gaameObject;
    [SerializeField] private GameObject gaaamObject;
    public void OnUnderstand()
    {
        Destroy(gaameObject);
        Destroy(gaaamObject);
    }
}
