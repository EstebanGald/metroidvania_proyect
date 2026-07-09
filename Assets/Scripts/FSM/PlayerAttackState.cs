using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackTimer;

    public PlayerAttackState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        //Wind up the timer
        attackTimer = player.attackDuration; 
        //Fire the animation exactly once
        player.anim.SetTrigger("isAttacking");
        //Turn ON the shield to block falling/running animations
        player.anim.SetBool("melee_attack", true);
        //Hard stop on the ground ---
        if (player.isGrounded)
        {
            // Instantly kill horizontal momentum, but keep vertical (gravity)
            player.body.velocity = new Vector2(0f, player.body.velocity.y);
        } 
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Tick down the timer
        attackTimer -= Time.deltaTime;

        //Exit the state when the animation finishes
        if (attackTimer <= 0f)
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
    //TheExit method runs the exact moment we leave this state
    public override void Exit()
    {
        base.Exit();
        
        // Turn OFF the shield so the player can run and fall normally again
        player.anim.SetBool("melee_attack", false); 

        //Kill any ghost inputs that got queued up
        player.anim.ResetTrigger("isAttacking");
    }
}