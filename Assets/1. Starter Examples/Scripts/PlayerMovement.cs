using UnityEngine;
using UnityEngine.InputSystem;

/// This script handles the player's movement, including walking, jumping, wall sliding, and wall jumping.
/// See these 2 YouTube videos to follow along with the code:
/// https://www.youtube.com/watch?v=xb3d7HarKcI
/// https://www.youtube.com/watch?v=OT537RfNzCU

public class PlayerMovement : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Animator animator;
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

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask wallLayer;
    private SpriteRenderer spriteRenderer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding = false;

    // Wall Jumping
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get flip component from the sprite player object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GroundCheck(); // this constantly checks if the player can jump and how many jumps are left
        ProcessGravity(); // this constantly checks the player's gravity
        ProcessWallSlide(); // this constantly checks if the player is wall sliding
        ProcessWallJump(); // this constantly checks if the player is wall jumping
       
        if (!isWallJumping)
        {
            // During a wall jump, we want to prevent the player from moving horizontally.
            // This ensures that the wall jump feels more controlled and directed.
            rb.linearVelocity  = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y); // here only the x velocity is controlled by keyborad input, the y velocity is controlled by gravity and jump force
             Flip(); // this constantly checks if the player is facing the right direction
        }
        animator.SetFloat("yVelocity", rb.linearVelocity.y); // this sets the speed parameter in the animator to the absolute value of the horizontal movement, so that the player can walk left and right
        //animator.SetFloat("magnitude",rb.linearVelocity.magnitude); // this sets the speed parameter in the animator to the absolute value of the horizontal movement, so that the player can walk left and right
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x)); // in comments found reference to this as "magnitude" not working.
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
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                //Light tap = half height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
                
            }
        }

        //Wall Jump
        if(context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); // this will make player jump away from the wall
            wallJumpTimer = 0f; // Reset the timer after performing a wall jump
            animator.SetTrigger("jump");

            //Force the flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                
            }


            Invoke(nameof(CancelWallJump), wallJumpTime +0.1f); // Wall jump will last 0.5 secondss and can jump again after 0.6 seconds
        }
    }

   


    private void GroundCheck()
    {
        //Check if the player is grounded by checking if the ground check position overlaps with any colliders in the ground layer
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer))
        //if(Physics2D.OverlapCircle(groundCheckPos.position, groundCheckSize.x, groundLayer)) // this is an alternate method to check if the player is grounded using a circle instead of a box
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            wallJumpTimer = 0f;

            // Landing always ends any in-progress wall jump, otherwise isWallJumping
            // can get stuck true (its Invoke(CancelWallJump) gets cancelled if the
            // player touches a wall again before it fires) and horizontal input
            // stays locked out even while standing on the ground.
            isWallJumping = false;
            CancelInvoke(nameof(CancelWallJump));
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        //Check if the player is on a wall by checking if the wall check position overlaps with any colliders in the wall layer
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer);
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

    private void ProcessWallSlide()
    {
        //Not grounded & on a wall & movement ! = 0
        if (!isGrounded && WallCheck() && horizontalMovement != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            wallJumpDirection = -transform.localScale.x; // Assuming the player is facing right when localScale.x is positive
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump)); // Cancel any previous invocation of CancelWallJump

        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

   

    private void CancelWallJump()
    {
        isWallJumping = false;
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
        
        if(isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            // Toggle the direction the player is facing.
            // If isFacingRight was true, it becomes false.
            // If it was false, it becomes true.
            isFacingRight = !isFacingRight;

            
            // This changes the player's X scale from 1 to -1 (or vice versa).
            // While this works, it also flips the entire GameObject,
            // including any child objects attached to it.
            //
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            // Newer and cleaner method:
            // Flip only the sprite's image without changing the GameObject's scale.
            // The '!' operator toggles the current value:
            // - If flipX is false, it becomes true.
            // - If flipX is true, it becomes false.
            //
            // This is generally the preferred approach when using a SpriteRenderer.
            //spriteRenderer.flipX = !spriteRenderer.flipX; // alternate method to flip the sprite without changing the scale of the player object
        }
    }

    // to be able to visulaize our ground check use Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.pink;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
