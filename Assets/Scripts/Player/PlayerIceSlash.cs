using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceSlash : MonoBehaviour
{
    private Animator animator;
    public AnimationClip anim;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Active(Vector2 direction)
    {
        this.transform.right = direction;
        
        animator.Play(anim.name);
    }
}
