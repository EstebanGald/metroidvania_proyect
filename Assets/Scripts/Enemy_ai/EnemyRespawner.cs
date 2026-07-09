using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDistance = 20f;

    private Transform playerTransform;
    private Vector3 spawnPosition;
    private bool isDead = false;

    private Health health;
    private EnemyBase enemyBase;
    private EnemyContactDamage enemyContactDamage;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;
    private Animator animator;
    private Rigidbody2D body;

    private void Awake()
    {
        health = GetComponent<Health>();
        enemyBase = GetComponent<EnemyBase>();
        enemyContactDamage = GetComponent<EnemyContactDamage>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        spawnPosition = transform.position;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;

        health.onDeath.AddListener(OnEnemyDied);
    }

    private void OnEnemyDied()
    {
        isDead = true;
        health.disableOnDeath = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (enemyBase != null)
            enemyBase.enabled = false;
        if (enemyContactDamage != null)
            enemyContactDamage.enabled = false;
        if (animator != null)
            animator.enabled = false;
        if (body != null)
            body.simulated = false;

        foreach (Collider2D col in colliders)
        {
            if (col != null)
                col.enabled = false;
        }

        transform.position = spawnPosition;
    }

    private void Update()
    {
        if (!isDead || playerTransform == null)
            return;

        if (Vector3.Distance(playerTransform.position, spawnPosition) >= respawnDistance)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        isDead = false;

        health.currentHealth = health.maxHealth;
        health.disableOnDeath = true;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (enemyBase != null)
            enemyBase.enabled = true;
        if (enemyContactDamage != null)
            enemyContactDamage.enabled = true;
        if (animator != null)
            animator.enabled = true;
        if (body != null)
            body.simulated = true;

        foreach (Collider2D col in colliders)
        {
            if (col != null)
                col.enabled = true;
        }
    }
}
