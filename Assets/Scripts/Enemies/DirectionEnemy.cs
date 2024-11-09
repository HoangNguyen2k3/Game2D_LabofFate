using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionEnemy : NetworkBehaviour
{    private SpriteRenderer bodyEnemy;
    private GameObject target;
    [SerializeField] private float distanceDetect;
    // NetworkVariable to sync flipX state across the network
    public NetworkVariable<bool> flipXState = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private void Start()
    {
        bodyEnemy = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!IsOwner) return; // Ensure only the owner can change the flipX

        if (target != null&&(Vector2.Distance(target.transform.position,transform.position)<=distanceDetect))
        {
            if (target.transform.position.x > gameObject.transform.position.x)
            {
                SetFlipX(false);
            }
            else
            {
                SetFlipX(true);
            }
        }
        else
        {
            // Find the player only if target is not assigned
            if (FindFirstObjectByType<PlayerController>())
            {
                target = FindFirstObjectByType<PlayerController>().gameObject;
            }
        }

        // Apply flipXState to the sprite renderer on all clients
        bodyEnemy.GetComponent<SpriteRenderer>().flipX = flipXState.Value;
    }

    // Method to set the flipX value and sync across network
    private void SetFlipX(bool flip)
    {
        if (flipXState.Value != flip)  // Only change if the value is different
        {
            flipXState.Value = flip;
            UpdateFlipXStateServerRpc(flip);  // Update flipX state across network
        }
    }

    [ServerRpc]
    private void UpdateFlipXStateServerRpc(bool flip)
    {
        bodyEnemy.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
