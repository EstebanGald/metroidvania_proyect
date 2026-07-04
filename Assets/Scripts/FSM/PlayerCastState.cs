using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastState : PlayerState
{
    private float castTimer;
    public PlayerCastState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        castTimer = player.castDuration; // Set the cast timer to the attack duration
        player.SpawnFireball(); // Spawn the fireball when entering the cast state
        // Trigger the casting animation
        player.anim.SetTrigger("isCasting");
        player.anim.SetBool("casting", true);
        if (player.isGrounded)
        {
            // Instantly kill horizontal momentum, but keep vertical (gravity)
            player.body.velocity = new Vector2(0f, player.body.velocity.y);
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        castTimer -= Time.deltaTime; // Decrease the cast timer
        if (castTimer <= 0f)
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
    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("casting", false);
        player.anim.ResetTrigger("isCasting");
    }
}
