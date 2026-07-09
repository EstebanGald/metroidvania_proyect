using UnityEngine;

public abstract class PlayerState
{
    protected PlayerMovement player; 
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    // Runs once the moment you SWITCH to this state
    public virtual void Enter() { }

    // Runs every single frame (Replaces your standard Update)
    public virtual void LogicUpdate() { }

    // Runs every physics step (Replaces FixedUpdate)
    public virtual void PhysicsUpdate() { }

    // Runs once the moment you LEAVE this state
    public virtual void Exit() { }
}