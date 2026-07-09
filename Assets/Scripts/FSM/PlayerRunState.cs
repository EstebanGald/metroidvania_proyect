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
        // If we are touching a ladder AND the player presses Up or Down
        float yInput = Input.GetAxisRaw("Vertical");

        if (player.IsTouchingClimbable() && Mathf.Abs(yInput) > 0.1f)
        {
            stateMachine.ChangeState(player.ClimbState);
            return; // Stop reading the rest of the logic
        }
        player.CheckForAttack();
        player.CheckForFireball();
        player.CheckForJump();
        if (!player.isGrounded)
        {
            stateMachine.ChangeState(player.AirState);
            return; // The 'return' keyword tells the code to stop reading the rest of this method
        }
        //ACCELERATION LOGIC -------------------
        //Calculate what speed the player *wants* to go
        float targetSpeed = player.horizontalInput * player.maxSpeed;
        
        //Smoothly ramp the current X velocity towards the targetSpeed
        float newVelocityX = Mathf.MoveTowards(player.body.velocity.x, targetSpeed, player.acceleration * Time.deltaTime);
        
        //Apply the new smooth velocity!
        player.body.velocity = new Vector2(newVelocityX, player.body.velocity.y);
        // -------------------------------

       //Flip the player sprite based on direction
        player.CheckForFlipping();

        //If the player lets go of the keys, switch back to Idle
        if (Mathf.Abs(player.horizontalInput) <= 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
