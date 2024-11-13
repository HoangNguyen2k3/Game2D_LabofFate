using Unity.Netcode;
using UnityEngine;

public class ActiveWeapon : NetworkBehaviour
{
    [SerializeField] private GameObject activeBow;
    [SerializeField] private GameObject activeSword;
    public NetworkVariable<bool> usingArbalet = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        activeBow = GameObject.FindGameObjectWithTag("activeFar");
        activeSword = GameObject.FindGameObjectWithTag("activeClose");
    }

    private void Start()
    {
        if (IsOwner)
        {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");
            activeSword.SetActive(true);
            activeBow.SetActive(false);
        }
        usingArbalet.OnValueChanged += OnWeaponChanged;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (activeBow==null || activeSword==null) {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");
        }
       
        // Toggle weapon on pressing Tab
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleWeaponServerRpc();
        }
    }

    private void OnWeaponChanged(bool oldValue, bool newValue)
    {
        if (activeBow == null || activeSword == null)
        {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");
        }
        activeSword.SetActive(!newValue);
        activeBow.SetActive(newValue);
    }
    [ServerRpc]
    private void ToggleWeaponServerRpc()
    {
        usingArbalet.Value = !usingArbalet.Value; // Syncs across clients
    }
}
