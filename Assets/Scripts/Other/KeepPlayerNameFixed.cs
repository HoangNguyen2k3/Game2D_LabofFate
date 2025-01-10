using TMPro;
using Unity.Netcode;
using UnityEngine;

public class KeepPlayerNameFixed : NetworkBehaviour
{
    public Transform player;
    private Vector3 initialScale;
    public TextMeshPro nameplayer;

    public PlayerSetting playerSetting;

    void Start()
    {
        initialScale = transform.localScale;
        nameplayer = GetComponent<TextMeshPro>();
    }

    void LateUpdate()
    {
        Vector3 playerScale = player.localScale;
        transform.localScale = new Vector3(
            Mathf.Sign(playerScale.x) * initialScale.x,
            Mathf.Sign(playerScale.y) * initialScale.y,
            initialScale.z
        );
    }
    private void Update()
    {
        if (nameplayer.text != playerSetting.networkPlayerName.Value.ToString())
        {
            nameplayer.text = playerSetting.networkPlayerName.Value.ToString();
        }
    }
}
