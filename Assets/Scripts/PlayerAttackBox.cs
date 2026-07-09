using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Did we hit something tagged "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            //Try to find the Health component on the thing we hit
            Health enemyHealth = collision.GetComponent<Health>();

            //If it actually has a Health component, tell it to take damage!
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}