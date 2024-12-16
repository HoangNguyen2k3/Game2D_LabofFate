using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Flash : NetworkBehaviour
{
    [SerializeField] private Material whiteFlash;
    [SerializeField] private float restoreDefaultMatTime = 0.3f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    public bool takedDamage = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }
    [ServerRpc]
    public void TriggerFlashServerRpc()
    {
        FlashEffectClientRpc();
    }
    [ClientRpc]
    private void FlashEffectClientRpc()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        takedDamage = true;
        spriteRenderer.material = whiteFlash;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
        takedDamage = false;
    }
}
