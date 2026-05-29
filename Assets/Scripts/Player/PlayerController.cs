using UnityEngine;

// Requires Rigidbody2D and Animator components on the same GameObject
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;      // Vertical impulse applied on jump
    [SerializeField] private int maxJumps = 1;           // Max jumps before landing (set to 2 for double jump)

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;       // Horizontal movement speed in units/second

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;          // Empty child GameObject positioned at the player's feet
    [SerializeField] private float groundCheckRadius = 0.1f; // Radius of the overlap circle used to detect the ground
    [SerializeField] private LayerMask groundLayer;          // Layers considered as ground

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;        // Raw horizontal axis input (-1, 0 or 1)
    private bool isGrounded;        // True when the ground check detects a ground collider
    private bool isFacingRight = true;
    private bool isDead;
    private int jumpsRemaining;     // Decrements on each jump, resets to maxJumps on landing
    public bool IsFacingRight => isFacingRight;
    private bool inputEnabled = true;



    // Pre-hashed animator parameter IDs for performance (avoids string lookups every frame)
    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashIsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashJump = Animator.StringToHash("Jump");
    private static readonly int HashRevive = Animator.StringToHash("Revive");



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.RegisterPlayer(gameObject);

    }

    private void Update()
    {
        if (isDead) return;
        if (!inputEnabled) return;

        // Read raw horizontal input (-1 = left, 0 = idle, 1 = right)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Jump input is handled in Update to avoid missing button presses between fixed frames
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
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
            jumpsRemaining = maxJumps;
    }

    // Sets horizontal velocity directly, preserving vertical velocity from gravity
    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    // Applies vertical impulse by overriding the Y velocity
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpsRemaining--;
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
    }

    // Called externally (e.g. by a hazard or kill zone) to trigger the death sequence
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Disables physics so the corpse doesn't slide
        anim.SetTrigger(HashDie);
        GameEvents.TriggerPlayerDied(); 
    }
    public void Revive()
    {
        isDead = false;
        anim.SetTrigger(HashRevive);
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

    // Draws the ground check radius in the Scene view for easier positioning
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }



}