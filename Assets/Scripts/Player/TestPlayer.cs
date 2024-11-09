using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    private Vector3 InputMove;
    private float speed = 10f;
    Vector3 otherPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            InputMove.x = Input.GetAxisRaw("Horizontal");
            InputMove.y = Input.GetAxisRaw("Vertical");
            transform.position += InputMove.normalized * speed * Time.deltaTime;
            SynPlayerPosServerRpc(transform.position);
        }
        else
        {
            transform.position = otherPos;
        }

    }
    [ServerRpc]
    void SynPlayerPosServerRpc(Vector3 pos)
    {
        otherPos = pos;
    }
}
