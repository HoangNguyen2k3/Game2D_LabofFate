using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public AnimationClip openAnim;
    public AnimationClip closeAnim;
    private Collider2D collider;

    public bool isOpen;

    private void Awake() {
        collider = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        animator.Play(openAnim.name);
        collider.enabled = false;
        isOpen = true;
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        animator.Play(closeAnim.name);
        collider.enabled = true;
        isOpen = false;
    }


}
