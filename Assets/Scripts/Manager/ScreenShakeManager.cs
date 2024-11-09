using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager :MonoBehaviour
{
    public static ScreenShakeManager instance;
    private CinemachineImpulseSource source;
    private void Awake()
    {
        instance = this;
        source = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeScreen()
    {
        source.GenerateImpulse();
    }
}
