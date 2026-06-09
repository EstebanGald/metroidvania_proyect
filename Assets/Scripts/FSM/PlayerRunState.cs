using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        // Guarantee we are unfrozen the exact millisecond we start running
        player.body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.CheckForAttack();
        //Check if the player is trying to jump! ---
        player.CheckForJump();
        //Check if we are falling or jumping ---
        if (!player.isGrounded)
        {
            stateMachine.ChangeState(player.AirState);
            return; // The 'return' keyword tells the code to stop reading the rest of this method
        }
        // -----------------------------------------------
        // --- NEW: ACCELERATION LOGIC -------------------
        // 1. Calculate what speed the player *wants* to go
        float targetSpeed = player.horizontalInput * player.maxSpeed;
        
        // 2. Smoothly ramp the current X velocity towards the targetSpeed
        float newVelocityX = Mathf.MoveTowards(player.body.velocity.x, targetSpeed, player.acceleration * Time.deltaTime);
        
        // 3. Apply the new smooth velocity!
        player.body.velocity = new Vector2(newVelocityX, player.body.velocity.y);
        // -------------------------------

       // 2. Flip the player sprite based on direction (Using the new helper method!)
        player.CheckForFlipping();

        // 3. If the player lets go of the keys, tell the Brain to switch back to Idle!
        if (Mathf.Abs(player.horizontalInput) <= 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
