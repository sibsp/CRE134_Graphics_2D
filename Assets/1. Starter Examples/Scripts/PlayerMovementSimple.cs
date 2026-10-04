using UnityEngine;
using UnityEngine.InputSystem;

/// This is a simplified version of PlayerMovement, with animations and wall sliding/wall jumping removed.
/// It covers walking, jumping, ground checking, and gravity - a good starting point for students.
/// See these 2 YouTube videos to follow along with the code:
/// https://www.youtube.com/watch?v=xb3d7HarKcI
/// https://www.youtube.com/watch?v=OT537RfNzCU

public class PlayerMovementSimple : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isFacingRight = true;
    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f;
    public float fallSpeedMultiplier = 2.0f;

    // Update is called once per frame
    void Update()
    {
        GroundCheck(); // this constantly checks if the player can jump and how many jumps are left
        ProcessGravity(); // this constantly checks the player's gravity

        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y); // here only the x velocity is controlled by keyboard input, the y velocity is controlled by gravity and jump force
        Flip(); // this constantly checks if the player is facing the right direction
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                //Hold down the jump button = full height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                //Light tap = half height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }

    private void GroundCheck()
    {
        //Check if the player is grounded by checking if the ground check position overlaps with any colliders in the ground layer
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        // Check if the player needs to turn around.
        // This happens in two situations:
        //
        // 1. The player is currently facing right, but starts moving left.
        // 2. The player is currently facing left, but starts moving right.
        //
        // If either of these conditions is true, we need to flip the sprite.

        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            // Toggle the direction the player is facing.
            isFacingRight = !isFacingRight;

            // This changes the player's X scale from 1 to -1 (or vice versa).
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // to be able to visualize our ground check use Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
