using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PositionName : NetworkBehaviour
{
    [SerializeField] private TextMeshPro namePlayer;

    private void Start()
    {
        if (namePlayer == null)
        {
            Debug.LogError("namePlayer is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (namePlayer != null)
        {
            // ??m b?o tên luôn ?i theo v? trí c?a nhân v?t
            namePlayer.transform.position = transform.position + Vector3.up; // Di chuy?n tên lên trên m?t chút
        }
    }
}
