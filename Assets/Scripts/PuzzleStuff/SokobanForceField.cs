using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanForceField : MonoBehaviour
{
    public bool isTriggered = false;
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("SokobanBox"))
        {
            SokobanBox sokobanBox = other.transform.GetComponent<SokobanBox>();
            // if (sokobanBox) sokobanBox.Reset();
            if (sokobanBox) isTriggered = true;
        }
    }

    public bool Triggered()
    {
        return isTriggered;
    }
}
