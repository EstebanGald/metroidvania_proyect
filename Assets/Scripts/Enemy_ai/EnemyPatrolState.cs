using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(EnemyBase enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //Check for the player
        if (enemy.isAggressive && enemy.DetectPlayer())
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return; // Stop reading the patrol logic
        }

        //Check the sensors: Did we hit a wall OR find a ledge?
        if (enemy.CheckForWall() || enemy.CheckForLedge())
        {
            // Turn around!
            enemy.Flip();
        }
        
        // 2. Keep walking forward
        enemy.body.velocity = new Vector2(enemy.patrolSpeed * enemy.facingDirection, enemy.body.velocity.y);
    }
}