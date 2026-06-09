using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackTimer;

    public EnemyAttackState(EnemyBase enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Stop moving to attack
        enemy.body.velocity = new Vector2(0f, enemy.body.velocity.y);
        
        // Set the timer (we can make this a variable in EnemyBase later)
        attackTimer = 1f; 
        
        // TODO: Fire attack animation here!
        Debug.Log("Enemy swings at the player!"); 
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            // Attack finished! Go back to chasing (which will instantly attack again if still in range)
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }
}