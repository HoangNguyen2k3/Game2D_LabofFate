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
            if (arpalet.activeSelf == false)
            {
                arpalet.SetActive(true);
            }

            return;
        }
        else
        {
            if(arpalet.activeSelf == true)
            arpalet.SetActive(false);
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
