using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("How many Hearts")]
    public int maxHealth = 3; 
    public int currentHealth;

    [Header("I-Frames (Invincibility)")]
    public float iFrameDuration = 1.5f; // How long you are invincible
    public int numberOfFlashes = 5;     // How many times the sprite blinks
    private bool isInvincible = false;
    private SpriteRenderer spriteRend;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    [Header("Death Behavior")]
    public bool disableOnDeath = true;


    private void Awake()
    {
        // Try to automatically find the SpriteRenderer on this object or its children
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Everyone starts at full health when they spawn
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        // 1. THE GATEKEEPER: If we are invincible, completely ignore the hit!
        if (isInvincible) return;
        // 1. Subtract the damage
        currentHealth -= damageAmount;
        
        // 2. Shout to the Inspector: "I took damage!"
        onTakeDamage.Invoke();

        // 3. Check if we died
        if (currentHealth <= 0)
        {
            Die();
        } else {
            // 5. Start the Invincibility timer and flashing effect!
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // A Coroutine allows us to pause code execution (like a timer) without pausing the whole game
    private IEnumerator InvincibilityRoutine()
    {
        // Turn ON invincibility
        isInvincible = true;

        // Make the sprite flash if we found a SpriteRenderer
        if (spriteRend != null)
        {
            for (int i = 0; i < numberOfFlashes; i++)
            {
                // Turn slightly transparent (Alpha 0.5)
                spriteRend.color = new Color(1, 1, 1, 0.5f);
                
                // Wait for a fraction of the total I-Frame time
                yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));

                // Turn solid white again (Alpha 1)
                spriteRend.color = Color.white;
                yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            }
        }
        else
        {
            // If there's no sprite renderer (like on a destructible crate), just wait out the timer
            yield return new WaitForSeconds(iFrameDuration);
        }

        // Turn OFF invincibility so they can be hurt again
        isInvincible = false;
    }

    private void Die()
    {
        onDeath.Invoke();
        
        //Destroy the object so it disappears from the game
        //TODO: Death animation
        //If GameObject is an enemy, disable it instead of destroying it so we can reuse it later
        if (disableOnDeath)
            gameObject.SetActive(false);
    }
}