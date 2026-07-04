using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    // Singleton for Checkpoint easy access
    public static RespawnManager instance; 

    [Header("Player References")]
    public Transform playerTransform;
    public Health playerHealth;
    
    // To refill hearts
    public PlayerHealthUI healthUI; 

    [Header("Respawn Data")]
    public Vector2 currentRespawnPoint;
    public float timeBeforeRespawn = 1.5f; // Wait time before teleporting

    public PlayerMovement playerMovement;

    private void Awake()
    {
        //Singleton setup
        if (instance == null) instance = this;
        
        // respawn where they started the level.
        currentRespawnPoint = playerTransform.position;
    }

    // Checkpoints will call this method when the player walks past them
    public void UpdateCheckpoint(Vector2 newPosition)
    {
        currentRespawnPoint = newPosition;
        Debug.Log("Checkpoint Saved at: " + newPosition);
    }

    // Link to Invoke Death event
    public void TriggerRespawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        //TODO: trigger fade-to-black animation
        
        //Wait time before respawn
        yield return new WaitForSeconds(timeBeforeRespawn);

        //Teleport player back to saved coordinates
        playerTransform.position = currentRespawnPoint;
        //reactivate the player
        playerTransform.gameObject.SetActive(true);
        playerMovement.ResetPlayerState(); 

        //Refill their health math
        playerHealth.currentHealth = playerHealth.maxHealth;

        //Tell the UI Canvas to update the heart sprites
        if (healthUI != null)
        {
            healthUI.UpdateHearts();
        }

        //TODO: Fade the screen back in
    }
}
