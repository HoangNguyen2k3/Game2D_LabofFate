using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject thingToFollow;

    Vector3 cameraOffset = new Vector3(0,0,-1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        if (thingToFollow != null)
        {
            transform.position = thingToFollow.transform.position + cameraOffset;    
        }
        
    }
}
