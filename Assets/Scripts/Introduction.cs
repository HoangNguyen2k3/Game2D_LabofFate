using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject gaameObject;
    public void OnUnderstand()
    {
        Destroy(gaameObject);
        Destroy(gameObject);
    }
}
