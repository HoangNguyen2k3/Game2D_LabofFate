using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SlashManagerCombo : NetworkBehaviour
{
    [field: SerializeField] public PlayerController PlayerController {get; protected set;}
    [SerializeField] private float attackCooldown = 0.7f;
    public NetworkVariable<bool> canAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isAttacking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> canCombo = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public Animator animator;
    private ActiveWeapon weapon;
    [SerializeField] private GameObject arpalet;

    public enum Element 
    {
        None,
        Lighting,
        Fire,
        Ice
    }

    public NetworkVariable< Element> currentElement = new NetworkVariable<Element>(Element.Fire,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public PlayerFireSlash fireSlash;
    public PlayerLightningSlash lightningSlash;
    public PlayerIceSlash iceSlash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fireSlash = GetComponentInChildren<PlayerFireSlash>();
        lightningSlash = GetComponentInChildren<PlayerLightningSlash>();
        iceSlash = GetComponentInChildren<PlayerIceSlash>();
    }

    private void Start()
    {
        if (IsOwner)
        {
            canAttack.Value = true;
            isAttacking.Value = false;
            canCombo.Value = false;
           // arpalet.SetActive(false);
        }
        arpalet.SetActive(false);
    }

    private void Update()
    {
        if (!IsOwner) return;
    //    if (canAttack.Value) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        bool facingRight = mousePos.x > playerScreenPoint.x;
      //  ChangeDirectionPlayerServerRpc(facingRight,OwnerClientId);

        if (weapon == null)
        {
            weapon = FindFirstObjectByType<ActiveWeapon>();
        }

        if (weapon.usingArbalet.Value)
        {
            if (!arpalet.activeSelf)
            {
                SetArpaletActiveServerRpc(true); 
            }
            return;
        }
        else
        {
            if (arpalet.activeSelf)
            {
                SetArpaletActiveServerRpc(false); 
            }
        }

        if (canAttack.Value && Input.GetMouseButtonDown(0))
        {
            canAttack.Value = false;
            isAttacking.Value = true;
            TriggerAttackServerRpc();
        }
    }

    public Vector2 MousePositionToUnitVector()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 dirToMouse = (mousePos - playerScreenPoint).normalized;
        Vector2[] directionVectors = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        foreach (Vector2 v in directionVectors)
        {
            float delta = Vector2.Dot(dirToMouse, v);
            if (delta >= Mathf.Sqrt(2)/ 2)
            {
                return v;
            }
        }
        return Vector2.down;
    }

    public string GetDirectionStr()
    {
        Dictionary<Vector2,string> vectorStrDict = new Dictionary<Vector2,string> {
            {Vector2.up, "Up"},
            {Vector2.down, "Down"},
            {Vector2.left, "Left"},
            {Vector2.right, "Left"},
        };

        vectorStrDict.TryGetValue(MousePositionToUnitVector(), out string str);
        if (str != null) return str;
        return PlayerController.DirectionStr;
    }
    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldown(attackCooldown));
    }
    private IEnumerator AttackCooldown(float time)
    {
        canAttack.Value = false;
        yield return new WaitForSeconds(time);
        canAttack.Value = true;
    }

    [ServerRpc]
    private void ChangeDirectionPlayerServerRpc(bool facingRight, ulong senderClientId)
    {
        ChangeDirectionPlayerClientRpc(facingRight, senderClientId);
    }

    [ClientRpc]
    private void ChangeDirectionPlayerClientRpc(bool facingRight, ulong targetClientId)
    {
        if (OwnerClientId == targetClientId)
        {
            transform.localScale = facingRight ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        }
    }

    [ServerRpc]
    private void TriggerAttackServerRpc()
    {
        //if (IsOwner) { canAttack.Value = true; }
        if (IsOwner)
        {
        isAttacking.Value = true;
        UpdateSlashRangeClientRpc(true);
        }

    }

    [ClientRpc]
    private void UpdateSlashRangeClientRpc(bool active)
    {
        if (IsOwner)
        {
            canAttack.Value = true;
        }
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
         //   FinalAttackClientRpc();
        }
    }

/*    [ClientRpc]
    private void FinalAttackClientRpc()
    {
        if (!IsOwner)
        {
            ScreenShakeManager.instance.ShakeScreen();
        }
    }*/
}
