using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public float mapWidth = 68f;

    void Start()
    {
        Camera minimapCamera = GetComponent<Camera>();

        float aspectRatio = (float)Screen.width / Screen.height;
        minimapCamera.orthographicSize = mapWidth / (2f * aspectRatio);
    }
}
