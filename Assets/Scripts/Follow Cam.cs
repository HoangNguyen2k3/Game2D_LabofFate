using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    private void LateUpdate()
    {
        CheckForPlayer();
    }
    private void CheckForPlayer()
    {
        if(player != null)
        {
            transform.position = player.position + new Vector3(0,0,-20);
        }
    }
}
