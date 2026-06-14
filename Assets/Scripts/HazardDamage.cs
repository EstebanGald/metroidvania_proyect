using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damageAmount);
            }

            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null) {
                playerMovement.TriggerKnockback();
            }
        }
    }
}
