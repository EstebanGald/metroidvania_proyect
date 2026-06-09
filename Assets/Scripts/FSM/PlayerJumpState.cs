using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 1. Apply the vertical jump force the exact moment we enter the state
        player.body.velocity = new Vector2(player.body.velocity.x, player.jumpPower);

        // 2. Immediately switch to the Air State so the player can steer and fall!
        stateMachine.ChangeState(player.AirState);
    }
}
