using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("ForwardMovement")]
    public float moveSpeed = 1f;
    public float acceleration = 3.5f;
    public bool moving = true;

    [Header("VerticalMovement")]
    public float verticalMoveSpeed = 1f;

    [Header("Attack")]
    public bool attack = false;

    public Rigidbody2D rigidBody;

    public Animator animator;

    public static GhostController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ForwardMovement();
        VerticalMovement();
        FeedAnimator();
    }

    [ContextMenu("FillComponents")]
    public void FillComponents() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Accelerates the enemy to the defined max speed
    /// </summary>
    public void ForwardMovement() {
        if (!moving) return;
        Vector2 tempVelocity = rigidBody.velocity;
        tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, moveSpeed, acceleration * Time.deltaTime);
        rigidBody.velocity = tempVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Constants.TAG_PLAYER)) {
            animator.SetTrigger(Constants.ANIM_GHOST_ATTACK);
        }
    }

    /// <summary>
    /// Stops the enemy from moving
    /// </summary>
    public void StopMovement() {
        moving = false;
        rigidBody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Stops the enemy from moving
    /// </summary>
    public void ResumeMovement() {
        moving = true;
    }

    /// <summary>
    /// Moves the enemy in the y axis following the player's movement
    /// </summary>
    public void VerticalMovement() {
        if (!moving) return;
        transform.position = new Vector3(transform.position.x,
                                         Mathf.MoveTowards(transform.position.y, PlayerController.instance.transform.position.y, verticalMoveSpeed * Time.deltaTime),
                                         transform.position.z);
    }

    public void FeedAnimator() {
        animator.SetFloat(Constants.ANIM_GHOST_VELOCITY, rigidBody.velocity.x);
    }
}
