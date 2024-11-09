using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Flash : NetworkBehaviour
{
    [SerializeField] private Material whiteFlash;
    [SerializeField] private float restoreDefaultMatTime = 0.3f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    // Ph??ng th?c này g?i t? client khi mu?n kích ho?t hi?u ?ng flash
    [ServerRpc]
    public void TriggerFlashServerRpc()
    {
        // Kích ho?t hi?u ?ng flash trên t?t c? các client
        FlashEffectClientRpc();
    }

    // Hi?u ?ng flash ??ng b? cho t?t c? các client
    [ClientRpc]
    private void FlashEffectClientRpc()
    {
        StartCoroutine(FlashRoutine());
    }

    // Coroutine ?? thay ??i màu và ph?c h?i màu g?c
    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlash;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }
}
