using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public AnimationClip openAnim;
    public AnimationClip closeAnim;
    private Collider2D collider;

    private void Awake() {
        collider = GetComponent<Collider2D>();
    }

    public bool isOpen;

    public void OpenDoor()
    {
        animator.Play(openAnim.name);
        collider.enabled = false;
        isOpen = true;
    }

    public void CloseDoor()
    {
        animator.Play(closeAnim.name);
        collider.enabled = true;
        isOpen = false;
    }


}
