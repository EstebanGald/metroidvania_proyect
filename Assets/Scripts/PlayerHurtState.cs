using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private float hurtTimer;

    public PlayerHurtState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // 1. Fire the hurt animation
        player.anim.SetTrigger("take_damage");

        // 2. Set the lockout timer
        hurtTimer = player.hurtDuration;

        // 3. Apply the knockback force! 
        // We push them UP, and horizontally OPPOSITE to the direction they are currently facing
        float knockbackDir = player.transform.localScale.x * -1f; // Reverses their facing direction
        player.body.velocity = new Vector2(player.knockbackForce.x * knockbackDir, player.knockbackForce.y);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Tick down the timer
        hurtTimer -= Time.deltaTime;

        // When the timer finishes, give control back to the player
        if (hurtTimer <= 0f)
        {
            if (player.isGrounded)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.AirState);
            }
        }
    }
}