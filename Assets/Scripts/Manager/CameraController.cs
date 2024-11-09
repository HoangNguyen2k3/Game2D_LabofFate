using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        if (virtualCamera.Follow == null && FindFirstObjectByType<PlayerController>())
        {
            virtualCamera.Follow = FindFirstObjectByType<PlayerController>().transform;
        }
    }
    private void Update()
    {
        if(virtualCamera.Follow==null&& FindFirstObjectByType<PlayerController>())
        {
            virtualCamera.Follow = FindFirstObjectByType<PlayerController>().transform;
        }
    }
}
