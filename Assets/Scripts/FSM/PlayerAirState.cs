using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        
    }
    public override void Enter()
    {      
        base.Enter();
        player.body.constraints = RigidbodyConstraints2D.FreezeRotation; // Ensure we don't have any X constraints when we enter the Air state
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // If we are touching a ladder AND the player presses Up or Down
        float yInput = Input.GetAxisRaw("Vertical");

        if (player.IsTouchingClimbable() && Mathf.Abs(yInput) > 0.1f)
        {
            stateMachine.ChangeState(player.ClimbState);
            return; // Stop reading the rest of the logic!
        }
        // --- Check if the player is trying to attack! ---
        player.CheckForAttack();
        player.CheckForFireball();
        //Check if the player is trying to jump! ---
        player.CheckForJump();

        //Air Momentum Logic ---
        // 1. Calculate what speed the player *wants* to go
        float targetSpeed = player.horizontalInput * player.maxSpeed;
        
        // 2. Smoothly ramp the current X velocity towards the targetSpeed
        float newVelocityX = Mathf.MoveTowards(player.body.velocity.x, targetSpeed, player.acceleration * Time.deltaTime);
        
        // 3. Apply the new steering velocity!
        player.body.velocity = new Vector2(newVelocityX, player.body.velocity.y);
        // -----------------------------------

       // 2. Flip the player sprite based on direction (Using the new helper method!)
        player.CheckForFlipping();

        //Gravity and Falling Logic -----------------------------------------------------
        if (player.body.velocity.y > 0f && !Input.GetKey(KeyCode.Space))
        {
            // 1. SHORT HOP: Player let go of space early while going UP
            player.body.gravityScale = player.defaultGravity * player.jumpEndEarlyGravityModifier;
        }
        else if (player.body.velocity.y < 0f)
        {
            // 2. HEAVY FALLING: Player is currently traveling DOWN
            player.body.gravityScale = player.defaultGravity * player.fallGravityMultiplier;

            // 3. TERMINAL VELOCITY: Clamp the fall speed so it never exceeds MaxFallSpeed
            // (Because falling velocity is a negative number, we use Mathf.Max to stop it from going lower)
            player.body.velocity = new Vector2(player.body.velocity.x, Mathf.Max(player.body.velocity.y, -player.maxFallSpeed));
        }
        else
        {
            // 4. NORMAL JUMP: Player is holding space and going UP
            player.body.gravityScale = player.defaultGravity;
        }
        // -------------------------------------------------------------------------------------------

        // 4. Check if we landed!
        if (player.isGrounded && player.body.velocity.y <= 0.1f)
        {
            // Reset gravity just in case we landed while doing a short hop
            player.body.gravityScale = player.defaultGravity; 

            // Decide whether to land running or land standing still
            if (Mathf.Abs(player.horizontalInput) > 0.01f)
            {
                stateMachine.ChangeState(player.RunState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}