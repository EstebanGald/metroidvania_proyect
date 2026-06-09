using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBase enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 1. Did the player run away out of our aggro range?
        if (!enemy.DetectPlayer())
        {
            stateMachine.ChangeState(enemy.PatrolState);
            return;
        }

        // 2. Did we get close enough to attack?
        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.AttackState);
            return;
        }

        // 3. Turn to face the player
        if (enemy.playerTarget.position.x > enemy.transform.position.x && enemy.facingDirection == -1)
            enemy.Flip();
        else if (enemy.playerTarget.position.x < enemy.transform.position.x && enemy.facingDirection == 1)
            enemy.Flip();

        // 4. Move towards the player (but stop if there is a cliff or wall!)
        if (enemy.CheckForWall() || enemy.CheckForLedge())
        {
            // Hard stop at the edge of the cliff, just glare at the player
            enemy.body.velocity = new Vector2(0f, enemy.body.velocity.y); 
        }
        else
        {
            // Run at chase speed!
            enemy.body.velocity = new Vector2(enemy.chaseSpeed * enemy.facingDirection, enemy.body.velocity.y);
        }
    }
}