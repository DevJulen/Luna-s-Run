using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullBlock : MonoBehaviour
{
    public bool moving = false;
    public float moveForce = 200;

    public float previousVelocity;

    public Rigidbody2D rigidBody;

    private void Start() {
        rigidBody = transform.parent.GetComponent<Rigidbody2D>();
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        CheckBlockCanMove();
    }

    private void LateUpdate()
    {
        RestrictBlockMovement();
        previousVelocity = rigidBody.velocity.x;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Constants.TAG_PLAYER)) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)  PlayerController.instance.inBlockRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG_PLAYER))
        {
            PlayerController.instance.inBlockRange = false;
            PlayerController.instance.blockCanBeMoved = false;
        }
    }

    /// <summary>
    /// Sets the block to non-movable again if it's been moved
    /// </summary>
    public void RestrictBlockMovement()
    {
        if (rigidBody.velocity.x < previousVelocity && rigidBody.velocity.x == 0f)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            PlayerController.instance.blockCanBeMoved = false;
        }
    }

    /// <summary>
    /// If the block can be moved removes all constraints allowing the player to move it
    /// </summary>
    public void CheckBlockCanMove()
    {
        if(PlayerController.instance.blockCanBeMoved) RemoveConstraints();
    }

    /// <summary>
    /// Removes all constraints in the rigid body and freezes the rotation in z. 
    /// </summary>
    private void RemoveConstraints() {
        rigidBody.constraints = RigidbodyConstraints2D.None;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
