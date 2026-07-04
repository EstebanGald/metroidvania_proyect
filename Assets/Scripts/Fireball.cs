using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    public float lifetime = 3f; // Se destruye tras 3 segundos si no choca con nada


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            // Ignorar colisiones con el jugador
            return;
        }
        // Si choca contra un enemigo que tenga tu script modular Health.cs
        if (hitInfo.CompareTag("Enemy"))
        {
            Health enemyHealth = hitInfo.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        //TODO: particle effect or sound effect on impact
        
        // Destruir la bola de fuego al impactar
        Destroy(gameObject);
    }
}
