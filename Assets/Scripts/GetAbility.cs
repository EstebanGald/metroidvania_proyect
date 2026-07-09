using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAbility : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    public AbilityNotifier abilityNotifier;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("VineGrow"))
            {
                player.vineGrowAbility = true;
                abilityNotifier.ShowVineGrow();
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Fireball"))
            {
                player.fireballAbility = true;
                abilityNotifier.ShowFireball();
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("DoubleJump"))
            {
                player.canDoubleJump = true;
                abilityNotifier.ShowDoubleJump();
                Destroy(gameObject);
            }
        }
    }
}
