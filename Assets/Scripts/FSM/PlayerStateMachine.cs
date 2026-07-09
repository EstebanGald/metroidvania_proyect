using UnityEngine;

public class PlayerStateMachine
{
    // This variable holds whatever state the player is currently in
    public PlayerState CurrentState { get; private set; }

    // This runs when the game first starts to put the player in a default state
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // This method will call to switch from Running to others
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();     // Clean up the old state
        CurrentState = newState; // Swap to the new state
        CurrentState.Enter();    // Set up the new state
    }
}
