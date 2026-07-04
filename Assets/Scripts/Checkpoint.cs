using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object that touched us has the "Player" tag...
        if (collision.CompareTag("Player"))
        {
            // Tell the RespawnManager to save this exact position!
            RespawnManager.instance.UpdateCheckpoint(transform.position);
        }
    }
}