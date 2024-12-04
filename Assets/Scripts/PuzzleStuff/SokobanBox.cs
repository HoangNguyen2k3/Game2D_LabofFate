using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanBox : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerMask;
    public Collider2D collision;
    public Collider2D hitbox;
    [field:SerializeField] DirectionSprite directionSprite;
    [field:SerializeField] public LaserEmitter emitter;
    enum Direction
    {up,down,left,right}
    [SerializeField] Direction direction;
    public bool isMoving {get; private set;}
    private Vector2 originalPos, targetPos; // pos to move the box
    public float timeToMove = 0.5f;
    public float emittingTime = 1.0f;
    public bool canMove = true;
    public bool isLaser = false;

    private Vector2 startingPos;

    private void Awake()
    {
        if(isLaser)
        {
            emitter = GetComponentInChildren<LaserEmitter>();
            directionSprite = GetComponentInChildren<DirectionSprite>();
        }
    }

    private void Start()
    {
        if(isLaser)
        {
            if((int)direction == 0)
            {
                directionSprite.inactiveSprite = directionSprite.inactiveSpriteUp;
                directionSprite.activeSprite = directionSprite.activeSpriteUp;
                emitter.direction = Vector2.up;
            }
            if((int)direction == 1)
            {
                directionSprite.inactiveSprite = directionSprite.inactiveSpriteDown;
                directionSprite.activeSprite = directionSprite.activeSpriteDown;
                emitter.direction = Vector2.down;
            }
            if((int)direction == 2)
            {
                directionSprite.inactiveSprite = directionSprite.inactiveSpriteLeft;
                directionSprite.activeSprite = directionSprite.activeSpriteLeft;
                emitter.direction = Vector2.left;
            }
            if((int)direction == 3)
            {
                directionSprite.inactiveSprite = directionSprite.inactiveSpriteRight;
                directionSprite.activeSprite = directionSprite.activeSpriteRight;
                emitter.direction = Vector2.right;
            }
        }
        startingPos = this.transform.position;
        emitter.position = this.transform.position;
    }

    private void Update()
    {
        if(isLaser)
        {
            Active_check();
        }
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

    public IEnumerator Emit()
    {
        DisableHitbox();

        emitter.active = true;
        yield return new WaitForSeconds(emittingTime);
        emitter.active = false;

        collision.enabled = true;
        EnableHitbox();
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

    public void Active_check()
    {
        if(emitter.active) directionSprite.Active();
        else directionSprite.InActive();
    }
}
