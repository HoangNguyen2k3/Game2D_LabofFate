using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Door : NetworkBehaviour
{
    public Animator animator;
    public AnimationClip openAnim;
    public AnimationClip closeAnim;
    private Collider2D collider2d;


    public NetworkVariable<bool> isOpen=new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isClose = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake() {
        collider2d = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (isOpen.Value)
        {
            OpenDoor();
            isOpen.Value = false; 
           
        }
        if (isClose.Value)
        {
            CloseDoor();
             isClose.Value= false;
        }
    }
    public void OpenDoor()
    {
        animator.Play(openAnim.name);
        collider2d.enabled = false;
    }

    public void CloseDoor()
    {
        animator.Play(closeAnim.name);
        collider2d.enabled = true;
    }


}
