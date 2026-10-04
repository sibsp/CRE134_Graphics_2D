using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static PlayerAttributes; // static import of PlayerAttributes class to access player attributes and functions

public class TopDownPlayerMovement : MonoBehaviour
{    
    Vector2 movement = Vector2.zero;
    [Range(10, 40)] public int speedMultiplier = 20; // speed multiplier for the player accessable in the inspector
    Rigidbody2D rb;
    Animator animator;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // initialise the animator parameters
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", -1);
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", false);
    }
    void Update()
    {
        if (animator.GetBool("isDead")) StartCoroutine(resetSceneOnDeath());
    }

    void FixedUpdate()
    {
        // int pSpeed = PlayerManager.Instance.speed;
        rb.AddForce(movement * speedMultiplier * plSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    void OnMovement(InputValue value)
    {   // Unity Input System function for movement - called from PlayerInput component
        movement = value.Get<Vector2>();
        
        if (!animator.GetBool("isDead"))
        {
            // if the player is moving (movement value is not zero), set the animator parameters
            if (movement != Vector2.zero)
            {
                animator.SetFloat("X", movement.x);
                animator.SetFloat("Y", movement.y);
                animator.SetBool("isWalking", true);
            } else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    // IEnumerator is a coroutine - a function that can be paused and resumed
    IEnumerator resetSceneOnDeath()
    {   
        // wait for the death animation to finish
        // then reset the scene
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        animator.SetBool("isDead", false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // reload scene
    }

    // on reloading the scene stuff - no needed so much but good to know about
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Reset(); // reset player attributes
    }


}
