using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    [Header("AI Type")]
    [Tooltip("If unchecked, this enemy will just pace back and forth (Tier 1). If checked, it will chase and attack (Tier 2).")]
    public bool isAggressive = false;
    
    [Header("Combat Settings")]
    public float aggroRange = 5f; // How close the player gets before the enemy chases
    public float attackRange = 1.2f; // How close the enemy needs to be to swing
    public float chaseSpeed = 5f; // Faster than patrol speed!
    public LayerMask whatIsPlayer; // Set this to your Player layer
    public Transform playerTarget { get; private set; } // Remembers where the player is

    [Header("Components")]
    public Rigidbody2D body;
    public Animator anim;

    [Header("Movement Settings")]
    public float patrolSpeed = 3f;
    public int facingDirection = 1; // 1 = right, -1 = left

    [Header("Sensors")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    
    public Transform ledgeCheck;
    public float ledgeCheckDistance = 0.5f;
    
    public LayerMask whatIsGround; // Tells the sensors what to look for

    private void Awake()
    {
        // Setup the state machine
        StateMachine = new EnemyStateMachine();
        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        // Start patrolling immediately!
        StateMachine.Initialize(PatrolState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    // --- SENSORS ---
    public bool CheckForWall()
    {
        // Shoots a laser forward. Returns TRUE if it hits a wall.
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    public bool CheckForLedge()
    {
        // Shoots a laser down. Returns TRUE if it hits NOTHING (meaning there is a ledge).
        return !Physics2D.Raycast(ledgeCheck.position, Vector2.down, ledgeCheckDistance, whatIsGround);
    }

    public void Flip()
    {
        facingDirection *= -1; // Flip the math
        transform.Rotate(0f, 180f, 0f); // Flip the physical sprite
    }

    public bool DetectPlayer()
    {
        // Draws an invisible circle. If the player steps inside, return TRUE.
        Collider2D hit = Physics2D.OverlapCircle(transform.position, aggroRange, whatIsPlayer);
        if (hit != null)
        {
            playerTarget = hit.transform;
            return true;
        }
        return false;
    }

    public bool IsPlayerInAttackRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        return hit != null;
    }

    // This draws lines in the Unity Editor so you can physically see the sensors!
    private void OnDrawGizmos()
    {
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * facingDirection * wallCheckDistance);
        if (ledgeCheck != null)
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.down * ledgeCheckDistance);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}