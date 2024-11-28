using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : MonoBehaviour
{

    public AnimationClip anim;
    private Animator animator;
    
    private Rigidbody2D body;

    public Transform point1;
    public Transform point2;

    public Vector2 direction;
    public float speed = 10;

    public Transform targetPos;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        this.transform.position = point1.position;
        animator.Play(anim.name);
    }

    private void Update() {
        
        Move();
    }

    private void Move()
    {
        if (Vector2.Distance(this.transform.position,point1.position)<1)

        {
            targetPos = point2;
        }

        if (Vector2.Distance(this.transform.position,point2.position)<1)
        {
            targetPos = point1;
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos.position, speed * Time.deltaTime);
    }
}


