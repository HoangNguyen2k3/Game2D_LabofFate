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

    private void Awake() {
        collider2d = GetComponent<Collider2D>();
    }
    [ServerRpc]
    public void OpenDoorServerRpc()
    {
        if (isOpen.Value) return;
        animator.Play(openAnim.name);
        collider2d.enabled = false;
        isOpen.Value = true;
    }

    public void CloseDoor()
    {
        if (!isOpen.Value) return;
        animator.Play(closeAnim.name);
        collider2d.enabled = true;
        isOpen.Value = false;
    }


}
