using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanForceField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("SokobanBox"))
        {
            SokobanBox sokobanBox = other.transform.GetComponentInParent<SokobanBox>();
            if (sokobanBox.hitbox.enabled)
            {
                sokobanBox.Reset();
            }
        }
    }
}
