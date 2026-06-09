using UnityEngine;

// Notice this does NOT inherit from MonoBehaviour!
public abstract class PlayerState
{
    protected PlayerMovement player; 
    protected PlayerStateMachine stateMachine;

    // This is a Constructor. It runs once when the state is created.
    public PlayerState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    // Runs once the moment you SWITCH to this state (e.g., playing an animation)
    public virtual void Enter() { }

    // Runs every single frame (Replaces your standard Update)
    public virtual void LogicUpdate() { }

    // Runs every physics step (Replaces FixedUpdate)
    public virtual void PhysicsUpdate() { }

    // Runs once the moment you LEAVE this state
    public virtual void Exit() { }
}