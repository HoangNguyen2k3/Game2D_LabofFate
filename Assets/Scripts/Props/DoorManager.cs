using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Door door1;
    public Door door2;

    private float timer = 0;
    private void Update() {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            door1.OpenDoor();
            door2.OpenDoor();
        }
    }

}
