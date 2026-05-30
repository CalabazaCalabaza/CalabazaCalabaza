using UnityEngine;

// Requires Rigidbody2D and Animator components on the same GameObject
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;      // Vertical impulse applied on jump

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;       // Horizontal movement speed in units/second

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;          // Empty child GameObject positioned at the player's feet
    [SerializeField] private float groundCheckRadius = 0.1f; // Radius of the overlap circle used to detect the ground
    [SerializeField] private LayerMask groundLayer;          // Layers considered as ground

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeDuration = 0.12f; // Segundos de gracia tras caer de un borde

    private float coyoteTimeCounter; // Countdown, > 0 significa que aún puede saltar
    private bool hasJumped; // True cuando el jugador saltó y aún no aterrizó

    private float moveInput;        // Raw horizontal axis input (-1, 0 or 1)
    private bool isGrounded;        // True when the ground check detects a ground collider
    private bool isFacingRight = true;
    private bool isDead;
    public bool IsFacingRight => isFacingRight;
    private bool inputEnabled = true;
    private float speedMultiplier = 1f;




    // Pre-hashed animator parameter IDs for performance (avoids string lookups every frame)
    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashIsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashJump = Animator.StringToHash("Jump");
    private static readonly int HashRevive = Animator.StringToHash("Revive");
    private static readonly int HashIsMoving = Animator.StringToHash("isMoving");
    private static readonly int HashIsJumping = Animator.StringToHash("isJumping");
    private static readonly int HashIsDeadBool = Animator.StringToHash("isDead");




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inputEnabled = true;
        GameManager.Instance.RegisterPlayer(gameObject);

    }

    private void Update()
    {
        if (isDead) return;
        if (!inputEnabled) return;

        // Read raw horizontal input (-1 = left, 0 = idle, 1 = right)
        moveInput = Input.GetAxisRaw("Horizontal");

        bool canJump = !hasJumped && (isGrounded || coyoteTimeCounter > 0f);

        if (Input.GetButtonDown("Jump") && canJump)
            Jump();

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // Physics operations run in FixedUpdate for consistent framerate-independent behavior
        CheckGround();
        Move();
    }

    // Casts an overlap circle at the feet of the player to determine if grounded
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool(HashIsGrounded, isGrounded);

        if (isGrounded)
        {
            hasJumped = false;
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    // Sets horizontal velocity directly, preserving vertical velocity from gravity
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed * speedMultiplier, rb.linearVelocity.y);

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    // Applies vertical impulse by overriding the Y velocity
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        hasJumped = true;
        coyoteTimeCounter = 0f;
        anim.SetTrigger(HashJump);
    }

    // Flips the sprite horizontally by inverting the X local scale
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    // Pushes movement data to the Animator each frame
    private void UpdateAnimator()
    {
        anim.SetFloat(HashSpeed, Mathf.Abs(moveInput));
        anim.SetBool(HashIsMoving, moveInput != 0f);
        anim.SetBool(HashIsJumping, !isGrounded);
    }

    // Called externally (e.g. by a hazard or kill zone) to trigger the death sequence
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetTrigger(HashDie);
        anim.SetBool(HashIsDeadBool, true);  // <--
        GameEvents.TriggerPlayerDied();
    }

    public void Revive()
    {
        isDead = false;
        anim.SetTrigger(HashRevive);
        anim.SetBool(HashIsDeadBool, false);  // <--
    }


    // Called via Animation Event on the last frame of the death animation
    public void OnDeathAnimationFinished()
    {
        GameManager.Instance.RespawnPlayer(); 
    }

    public void SetInputEnabled(bool value)
    {
        inputEnabled = value;
        if (!value)
        {
            moveInput = 0f;
            rb.linearVelocity = Vector2.zero;
            UpdateAnimator();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    // Draws the ground check radius in the Scene view for easier positioning
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }



}