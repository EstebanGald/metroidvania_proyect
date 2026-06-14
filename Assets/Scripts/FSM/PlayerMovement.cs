using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Gizmo Settings")]
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallCheckYOffset = -0.2f; 

    [Header("Movement")]
    public float maxSpeed = 10f; // The top speed the character can reach
    public float acceleration = 60f; // How fast they reach top speed
    public float deceleration = 60f; // How fast they slide to a stop

    [Header("Attacking")]
    public float attackDuration = 0.4f; // How long the attack state lasts
    private float lastAttackTime; //For attack buffering

    [Header("Hurt & Knockback")]
    public float hurtDuration = 0.4f; // How long the player is stunned
    public Vector2 knockbackForce = new Vector2(5f, 5f); // X is pushback, Y is upward bounce

    [Header("Jumping")]
    public float jumpPower;
    [SerializeField] private bool canDoubleJump;
    //Falling Physics ---
    [Tooltip("Multiplier applied to gravity when falling down")]
    public float fallGravityMultiplier = 2f; 
    
    [Tooltip("The absolute maximum speed the player can fall")]
    public float maxFallSpeed = 25f; 
    // ----------------------------
    
    // --- Coyote Time Settings ---
    [Tooltip("How much time the player has to jump after walking off a ledge")]
    [SerializeField] private float coyoteTime = 0.2f; 
    private float coyoteTimeCounter;
    // ---------------------------------

    // --- Jump Buffer Settings ---
    [Tooltip("How long the game remembers you pressed jump before hitting the ground")]
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    // ---------------------------------

    //Variable Jump Height Settings ---
    [Tooltip("The gravity multiplier added when jump is released early")]
    public float jumpEndEarlyGravityModifier = 3f;
    public float defaultGravity; // Used to remember what your normal gravity is
    // ------------------------------------------

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); //(Width, Height)
    [SerializeField] private LayerMask whatIsGround;

    public Animator anim;
    public Rigidbody2D body;
    [HideInInspector] public float horizontalInput; //We make this public so our states can read it
    [SerializeField] private int jumpsLeft;
    public bool isGrounded;

    //STATE MACHINE VARIABLES ---
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; } 
    public PlayerAirState AirState { get; private set; } 
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Remember normal gravity ---
        // We save whatever gravity scale you set in the Inspector so we can revert to it
        defaultGravity = body.gravityScale; 
        // ------------------------------------
        //CREATE THE BRAIN AND THE STATE ---
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine); 
        AirState = new PlayerAirState(this, StateMachine); 
        JumpState = new PlayerJumpState(this, StateMachine); 
        AttackState = new PlayerAttackState(this, StateMachine);
        HurtState = new PlayerHurtState(this, StateMachine);
        // -------------------------------------------
    }
    // --- NEW: START METHOD ---
    private void Start()
    {
        // When the game starts, force the player into the Idle State
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        //Read the input here, so all states can see what the player is pressing
         horizontalInput = Input.GetAxisRaw("Horizontal");
         
        //RUN THE STATE MACHINE ---
        // This tells whatever state is currently active to run its LogicUpdate()
        StateMachine.CurrentState.LogicUpdate();

        // Ground Check logic (using a Box for even footing)
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, whatIsGround);

        // Reset jumps AND reset Coyote Time when grounded (with the 0.1f micro-bounce fix!)
        if (isGrounded && body.velocity.y <= 0.1f)
        {
            coyoteTimeCounter = coyoteTime; 
            jumpsLeft = canDoubleJump ? 2 : 1;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; 
        }

        // Jump Buffer Timer Logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; 
        }
        
        

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", body.velocity.y);
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }

        Gizmos.color = Color.blue;
        Vector2 wallCheckOrigin = new Vector2(transform.position.x, transform.position.y + wallCheckYOffset);
        Vector2 rayDirection = Vector2.right * transform.localScale.x;
        Gizmos.DrawLine(wallCheckOrigin, wallCheckOrigin + (rayDirection * wallCheckDistance));
    }
    public void CheckForFlipping()
    {
        if (horizontalInput > 0.01f)
        {            
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void CheckForAttack()
    {
        // Check if 'X' is pressed AND enough time has passed since the last attack started
        // (Duration + a tiny 0.05 second buffer for the Animator)
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= lastAttackTime + attackDuration + 0.1f) 
        {
            // Record the exact time this new attack started
            lastAttackTime = Time.time; 
            
            StateMachine.ChangeState(AttackState);
        }
    }
    public void CheckForJump()
    {
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                jumpBufferCounter = 0f; 
                coyoteTimeCounter = 0f; 
                jumpsLeft--; 
                Debug.Log("Jumped! Jumps left: " + jumpsLeft);
                StateMachine.ChangeState(JumpState);
                
            }
            else if (jumpsLeft > 0 && canDoubleJump)
            {
                jumpBufferCounter = 0f;
                jumpsLeft--;
                anim.SetTrigger("DoubleJump");
                StateMachine.ChangeState(JumpState);
               
            }
        }
    }
    //"BRIDGE" METHOD:
    public void TriggerKnockback()
    {
        // We only want to get knocked back if we aren't already hurt!
        if (StateMachine.CurrentState != HurtState)
        {
            StateMachine.ChangeState(HurtState);
        }
    }
    //test
}