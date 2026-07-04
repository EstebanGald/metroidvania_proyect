using UnityEngine;

public class PlayerClimbState : PlayerState
{
    // FIX: Changed "Player" to "PlayerMovement" to match your actual script!
    public PlayerClimbState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // 1. Kill all momentum so we don't slide up the ladder
        player.body.velocity = Vector2.zero; 
        
        // 2. Turn off gravity so we don't slide down!
        player.body.gravityScale = 0f; 
        // 3. Set the climbing animation
        player.anim.SetBool("isClimbing", true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 1. Get Vertical Input (W/S or Up/Down arrows)
        float yInput = Input.GetAxisRaw("Vertical");

        // 2. Apply Vertical Movement (and lock horizontal movement if desired)
        player.body.velocity = new Vector2(0, yInput * player.climbSpeed);
        // 2.5. Set the climbing animation speed
        player.anim.SetFloat("climbVelocity", Mathf.Abs(yInput));

        // 3. BAILOUT: If we jump off the ladder
        if (Input.GetButtonDown("Jump"))
        {
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        // 4. BAILOUT: If we reach the top or bottom and leave the climbable zone
        if (!player.IsTouchingClimbable())
        {
            // Did we touch the ground? Go to Idle. Otherwise, we are in the air.
            if (player.isGrounded)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // CRITICAL: Turn gravity back on when we leave this state!
        player.body.gravityScale = player.defaultGravity; 
        // Reset the climbing animation
        player.anim.SetBool("isClimbing", false);
    }
}