using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float yInput = Input.GetAxisRaw("Vertical");
        if (player.IsTouchingClimbable() && Mathf.Abs(yInput) > 0.1f)
        {
            stateMachine.ChangeState(player.ClimbState);
            return;
        }
        player.CheckForAttack();
        player.CheckForFireball();
        if (Mathf.Abs(player.horizontalInput) > 0.01f) // If the player presses left or right, switch to the Run State
        {
            stateMachine.ChangeState(player.RunState);
            return; // The return keyword tells the code to stop reading the rest of this method
        }
        player.CheckForJump();
        if (stateMachine.CurrentState != this)
            return;
        //Check if we are falling or jumping 
        if (!player.isGrounded)
        {
            stateMachine.ChangeState(player.AirState);
            return; // The return keyword tells the code to stop reading the rest of this method
        }
        //DECELERATION (SLIDING) LOGIC
        // Smoothly ramp the current X velocity towards 0
        float newVelocityX = Mathf.MoveTowards(player.body.velocity.x, 0f, player.deceleration * Time.deltaTime);
        
        // Apply the sliding velocity
        player.body.velocity = new Vector2(newVelocityX, player.body.velocity.y);
        // -----------------------------------------
        //Anti sliding logic ---
        if (Mathf.Abs(player.body.velocity.x) < 0.01f)
        {
            // The slide is finished Freeze the X position so slopes/seams can't push the player.
            player.body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        // -----------------------------------------------
    }

    public override void Exit()
    {
        base.Exit();
        // UNFREEZE X POSITION 
        // This runs the exact millisecond we leave Idle to Run, Jump, or get Hurt.
        // We set the constraints back to ONLY FreezeRotation.
        player.body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}