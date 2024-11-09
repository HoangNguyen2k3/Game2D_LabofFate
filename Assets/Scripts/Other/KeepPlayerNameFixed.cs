using UnityEngine;

public class KeepPlayerNameFixed : MonoBehaviour
{
    public Transform player; // Gán player c?a b?n vào ?ây
    private Vector3 initialScale;

    void Start()
    {
        // L?u l?i kích th??c ban ??u c?a PlayerName
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // L?y localScale c?a player
        Vector3 playerScale = player.localScale;

        // B? qua vi?c ??o chi?u x ho?c y n?u có
        transform.localScale = new Vector3(
            Mathf.Sign(playerScale.x) * initialScale.x,
            Mathf.Sign(playerScale.y) * initialScale.y,
            initialScale.z
        );

/*        // Gi? nguyên v? trí ?? PlayerName luôn theo v? trí c?a player
        transform.position = player.position;*/
    }
}
