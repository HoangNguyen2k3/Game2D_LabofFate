using Unity.Netcode;
using UnityEngine;

public class ActiveWeapon : NetworkBehaviour
{
    [SerializeField] private GameObject activeBow;
    [SerializeField] private GameObject activeSword;
    public NetworkVariable<bool> usingArbalet = new NetworkVariable<bool>(false);
    private void Awake()
    {
        activeBow = GameObject.FindGameObjectWithTag("activeFar");
        activeSword= GameObject.FindGameObjectWithTag("activeClose");
    }
    private void Start()
    {
        if (IsOwner)
        {
            Debug.Log("player");
            activeSword.SetActive(true);
            activeBow.SetActive(false);
        }
        usingArbalet.OnValueChanged += OnWeaponChanged;
    }

    private void Update()
    {
        if (activeBow==null && activeSword==null)
        {
            activeBow = GameObject.FindGameObjectWithTag("activeFar");
            activeSword = GameObject.FindGameObjectWithTag("activeClose");
        }
        if (!IsOwner) return; 

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleWeaponServerRpc();
        }
    }

    private void OnWeaponChanged(bool oldValue, bool newValue)
    {
        activeSword.SetActive(!newValue);
        activeBow.SetActive(newValue);
    }

    [ServerRpc]
    private void ToggleWeaponServerRpc()
    {
        usingArbalet.Value = !usingArbalet.Value;
    }
}
