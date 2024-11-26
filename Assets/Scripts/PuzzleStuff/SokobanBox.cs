using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanBox : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerMask;
    public Collider2D collision;
    public Collider2D hitbox;
    public bool isMoving {get; private set;}
    private Vector2 originalPos, targetPos; // pos to move the box
    public float timeToMove = 0.5f;
    public bool canMove = true;

    private Vector2 startingPos;

    private void Start()
    {
        startingPos = this.transform.position;
    }

    public IEnumerator Move(Vector2 _direction)
    {
        if (CheckObstacle(_direction) || !canMove)
        {
            yield break;
        }
        Debug.Log("HEy");
        isMoving = true;
        collision.enabled = false;
        DisableHitbox();

        float elapsedTime = 0;
        originalPos = transform.position;
        targetPos = originalPos + _direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector2.Lerp(originalPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        collision.enabled = true;
        EnableHitbox();
        isMoving = false;
    }

    private bool CheckObstacle(Vector2 _direction)
    {
        RaycastHit2D rayInDir = Physics2D.Raycast((Vector2)this.transform.position + _direction , _direction, 0.1f, layerMask);
        RaycastHit2D rayInOppositeDir = Physics2D.Raycast((Vector2)this.transform.position - _direction , -_direction, 0.1f, layerMask);

        if (rayInDir.collider || rayInOppositeDir.collider)
        {
            return true;
        }
        return false;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void Reset()
    {
        if ((Vector2)this.transform.position != startingPos)
        {
            animator.Play("SokobanBoxDisappear");
        }
    }

    public void BackToStartingPos()
    {
        transform.position = startingPos;
    }
}
