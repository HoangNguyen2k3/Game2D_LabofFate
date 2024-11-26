using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class SokobanHitbox : MonoBehaviour
{
    public SokobanBox sokobanBox;

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("PlayerSlash") && !sokobanBox.isMoving)
        {
            Vector2 direction = GetDirection(other.transform.parent.transform);
            StartCoroutine(sokobanBox.Move(direction));
        }
    }

    private Vector2 GetDirection(Transform _playerHitboxPos)
    {
        Vector2 vectorToPlayerHitbox = this.transform.position - _playerHitboxPos.position;
        vectorToPlayerHitbox.Normalize();

        Vector2[] directionVectors = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        foreach (Vector2 v in directionVectors)
        {
            float delta = Vector2.Dot(vectorToPlayerHitbox, v);
            if (delta >= Mathf.Sqrt(2)/ 2)
            {
                return v;
            }
        }
        return Vector2.zero;
    }
}
