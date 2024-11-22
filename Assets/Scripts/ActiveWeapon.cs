using Unity.Netcode;
using UnityEngine;

public class ActiveWeapon : NetworkBehaviour
{
    [SerializeField] private GameObject activeBow;
    [SerializeField] private GameObject activeSword;

    public NetworkVariable<bool> usingArbalet = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private void Start()
    {
        if (IsOwner)
        {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");

            if (activeSword != null) activeSword.SetActive(true);
            if (activeBow != null) activeBow.SetActive(false);
            usingArbalet.OnValueChanged += OnWeaponChanged;
        }

    }

    private void Update()
    {
        if (!IsOwner) return; 

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleWeapon(); 
        }
    }

    private void ToggleWeapon()
    {
        usingArbalet.Value=!usingArbalet.Value;
    }

    [ServerRpc]
    private void ToggleWeaponServerRpc(bool newValue)
    {
        usingArbalet.Value = newValue; 
    }

    private void OnWeaponChanged(bool oldValue, bool newValue)
    {
        if (activeBow == null || activeSword == null)
        {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");
        }
        if (activeSword != null) activeSword.SetActive(!newValue);
        if (activeBow != null) activeBow.SetActive(newValue);
    }
}
