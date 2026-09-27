using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region Movement Variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    float moveInput;    // Horizontal input for movement
    bool isFlippedX = false;
    bool playerFrozen = false;
    int electricPlantKnockBackForce = 200;
    #endregion
    #region Ground Check Variables
    public LayerMask groundLayer;   // LayerMask to determine what is considered ground
    public float groundCheckRadius = 0.1f; // Radius of the overlap circle to determine if grounded
    public Color groundCheckColor = Color.red; // Color of the debug round check circle
    [SerializeField] private bool isGrounded;   // Is the character on the ground?
    #endregion
    #region Audio
    // sounds 
    public AudioClip jumpSound;
    public AudioClip zapSound;
    public AudioClip collectSound;
    #endregion
    #region  Component References
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Animator anim;  // Reference to the Animator component
    private SpriteRenderer sr;  // Reference to the SpriteRenderer component
    #endregion
    #region Prefabs
    public GameObject explosionEffect; // reference to the explosion effect prefab
    public GameObject puffCloudEffect; // reference to the puff cloud effect prefab
    #endregion
    public Transform spawnPointSecretArea; // secret area spawn point
    void Start()
    {
        // Get references to the components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {     
        // Check if the character is on the ground
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("jump", false);
            
            if (!playerFrozen) 
                {
                // Move the character
                // Get horizontal input
                moveInput = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
                if (rb.linearVelocity.x != 0) 
                {
                    anim.SetBool("isWalking", true);    // Set the walking animation to true
                    if (rb.linearVelocity.x > 0) {
                        sr.flipX = false;               // Flip the sprite based on the direction
                        isFlippedX = false;
                    }
                    else {
                        sr.flipX = true;
                        isFlippedX = true;
                    }
                }
                else anim.SetBool("isWalking", false);  // Set the walking animation to false

                // Handle jumping
                if (Input.GetButtonDown("Jump"))
                {
                    AudioSystem.Instance.PlaySound(jumpSound, transform.position, 0.1f); // Play the jump sound using the static AudioSystem object instance
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);                 // Set the y velocity to the jump force 
                    anim.SetBool("isGrounded", false);
                    anim.SetBool("jump", true);
                }
            }           
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ElectricPlant")) // Player hit the electric plant
        {
            Debug.Log("Player hit the electric plant");
            AudioSystem.Instance.PlaySound(zapSound, transform.position, 0.2f); // Play the jump sound using the static AudioSystem object instance
                
            GameManager.Instance.PlayerHealth -= 10;
            Debug.Log(GameManager.Instance.PlayerHealth);

            PlayerKnockBack(electricPlantKnockBackForce);

            // instatiate the puff cloud effect at the player's position
            Instantiate(puffCloudEffect, other.transform.position, Quaternion.identity);


        }
        else if (other.CompareTag("Portal"))  // Jump to secret area
        {
            if (GameManager.Instance.PlayerScore >= 50) {
                Debug.Log("Portal Jump!");
                rb.linearVelocity = Vector2.zero;
                transform.position = spawnPointSecretArea.position;
                StartCoroutine(WaitBeforeMovingAgain(1));
                Instantiate(puffCloudEffect, transform.position, Quaternion.identity);
            }
        }
    }

    // on collision enter with a collectable add 10 points to score
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Collectable")) {
            GameManager.Instance.PlayerScore += 10;
            AudioSystem.Instance.PlaySound(collectSound, transform.position, 1f); 
            Destroy(other.gameObject); // destroy the collectable object
            // instatiate the explosion effect at the collectable object's position
            Instantiate(explosionEffect, other.transform.position, Quaternion.identity);
        }
        else if (other.gameObject.CompareTag("SlimeBasic")) {
            GameManager.Instance.PlayerHealth -= 10;
            Debug.Log(GameManager.Instance.PlayerHealth);
            AudioSystem.Instance.PlaySound(zapSound, transform.position, 0.2f); // Play the jump sound using the static AudioSystem object instance
            PlayerKnockBack();
        }
    }


    public void PlayerKnockBack(int knockBackForce = 200)
    {
            if (!isFlippedX) {
                Debug.Log("Is Flipped on X axis: " + isFlippedX);
                // move player to the right with an impulse force
                rb.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            }
            else {
                // move player to the left with an impulse force
                rb.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);
            }
            StartCoroutine(WaitBeforeMovingAgain(1));
    }
    // a coroutine to wait for a few seconds before letting the player move again
    IEnumerator WaitBeforeMovingAgain(float delay) {
        playerFrozen = true;
        yield return new WaitForSeconds(delay);   
        playerFrozen = false;
    }

    // Draw the ground check circle for debugging. This is only visible in the Scene view.
    void OnDrawGizmos()
    {
        // Draw the ground check circle for debugging
        Gizmos.color = groundCheckColor;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
