using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player;
    public KeyUI keyUI;
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected) return;

        collected = true;
        player.keysCollected++;
        player.onKeyCollected?.Invoke();
        keyUI.UpdateKeyText();

        if (player.keysCollected >= player.keysRequired)
            player.onAllKeysCollected?.Invoke();

        Destroy(gameObject);
    }
}
