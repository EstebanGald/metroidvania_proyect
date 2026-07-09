using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 1;

    // This fires the exact frame the player bumps into the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryToDamage(collision.gameObject);
    }

    // This fires continuously if the player is pinned against the enemy
    private void OnCollisionStay2D(Collision2D collision)
    {
        TryToDamage(collision.gameObject);
    }

    //helper method
    private void TryToDamage(GameObject hitObject)
    {
        //hit the Player?
        if (hitObject.CompareTag("Player"))
        {
            //Look for the Health script on the Player
            Health playerHealth = hitObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}