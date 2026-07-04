using UnityEngine;

public class VineTriggerZone : MonoBehaviour
{
    [Header("Vine Reference")]
    [Tooltip("The vine that will start growing when the player enters this zone")]
    public VineGrow vineGrow;
    private bool playerInZone = false;

    [Header("Settings")]
    [Tooltip("Should this zone destroy itself after triggering?")]
    public bool destroyOnTrigger = true;

    public PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;
    }
    private void Update()
    {
        if (player.vineGrowAbility && playerInZone && Input.GetKeyDown(KeyCode.K) && vineGrow != null)
        {
            vineGrow.StartGrowing();
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
