using UnityEngine;

// Notice this also does NOT inherit from MonoBehaviour!
public class PlayerStateMachine
{
    // This variable holds whatever state the player is currently in
    public PlayerState CurrentState { get; private set; }

    // This runs when the game first starts to put the player in a default state (like Idle)
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // This is the magic method we will call to switch from Running to Jumping, etc.
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();     // Clean up the old state
        CurrentState = newState; // Swap to the new state
        CurrentState.Enter();    // Set up the new state
    }
}
