using UnityEngine;

public class PlayerVineCastState : PlayerState
{
    private float castTimer;

    public PlayerVineCastState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        castTimer = player.castDuration;

        if (player.currentVineGrow != null)
            player.currentVineGrow.StartGrowing();

        player.anim.SetTrigger("isCasting");
        player.anim.SetBool("casting", true);

        if (player.isGrounded)
            player.body.velocity = new Vector2(0f, player.body.velocity.y);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        castTimer -= Time.deltaTime;
        if (castTimer <= 0f)
        {
            if (player.isGrounded)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("casting", false);
        player.anim.ResetTrigger("isCasting");
        player.currentVineGrow = null;
    }
}
