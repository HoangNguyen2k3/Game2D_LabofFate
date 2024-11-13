using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SlashManagerCombo : NetworkBehaviour
{
    public NetworkVariable<bool> canAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> canCombo = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Animator animator;
    [SerializeField] private GameObject slashRange;
    private ActiveWeapon weapon;
    [SerializeField] private GameObject arpalet;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        bool facingRight = mousePos.x > playerScreenPoint.x;
        SetDirectionServerRpc(facingRight);

        if (weapon == null)
        {
            weapon = FindFirstObjectByType<ActiveWeapon>();
        }

        if (weapon.usingArbalet.Value)
        {
            if (!arpalet.activeSelf)
            {
                SetArpaletActiveServerRpc(true); // Notify the server to activate arpalet across all clients
            }
            return;
        }
        else
        {
            if (arpalet.activeSelf)
            {
                SetArpaletActiveServerRpc(false); // Notify the server to deactivate arpalet across all clients
            }
        }

        slashRange.SetActive(canAttack.Value);

        if (Input.GetMouseButtonDown(0) && !canCombo.Value && IsOwner)
        {
            canAttack.Value = true;
            TriggerAttackServerRpc();
        }
    }

    [ServerRpc]
    private void TriggerAttackServerRpc()
    {
        canAttack.Value = true;
        UpdateSlashRangeClientRpc(true);
    }

    [ClientRpc]
    private void UpdateSlashRangeClientRpc(bool active)
    {
        slashRange.SetActive(active);
    }

    [ServerRpc]
    private void SetDirectionServerRpc(bool facingRight)
    {
        SetDirectionClientRpc(facingRight);
    }

    [ClientRpc]
    private void SetDirectionClientRpc(bool facingRight)
    {
        transform.localScale = facingRight ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
    }

    [ServerRpc]
    private void SetArpaletActiveServerRpc(bool isActive)
    {
        SetArpaletActiveClientRpc(isActive);
    }

    [ClientRpc]
    private void SetArpaletActiveClientRpc(bool isActive)
    {
        arpalet.SetActive(isActive);
    }

    public void FinalAttack()
    {
        if (IsOwner)
        {
            ScreenShakeManager.instance.ShakeScreen();
            FinalAttackClientRpc();
        }
    }

    [ClientRpc]
    private void FinalAttackClientRpc()
    {
        if (!IsOwner)
        {
            ScreenShakeManager.instance.ShakeScreen();
        }
    }
}
