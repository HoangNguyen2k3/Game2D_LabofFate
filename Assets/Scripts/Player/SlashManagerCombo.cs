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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // G?i ServerRpc ?? yêu c?u c?p nh?t h??ng quay c?a nhân v?t
        bool facingRight = mousePos.x > playerScreenPoint.x;
        SetDirectionServerRpc(facingRight);

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
        // Server g?i ClientRpc ?? c?p nh?t h??ng quay trên t?t c? các client
        SetDirectionClientRpc(facingRight);
    }

    [ClientRpc]
    private void SetDirectionClientRpc(bool facingRight)
    {
        transform.localScale = facingRight ? new Vector3(1.2f, 1.2f, 1f) : new Vector3(-1.2f, 1.2f, 1f);
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
