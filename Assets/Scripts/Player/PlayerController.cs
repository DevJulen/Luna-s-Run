using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    public bool canMove = true;
    public float moveSpeed = 1.5f;
    public Rigidbody2D rigidBody;
    public bool moving;
    // offset to move the sprite locally when moving left and right
    public float spriteOffset = 0.01f;

    [Header("Jump")]
    public float jumpForce = 5f;
    // if the player is falling
    public bool falling = false;
    // if the player has jumped and is ascending
    public bool jumped = false;
    // seconds that the player will spend ascending
    public float ascendTime = 0.5f;
    // counter for the time that the player has spend ascending
    private float ascendCounter = 0f;
    // maximum falling speed
    public float maxFallSpeed = -5f;
    // time that the player will spend in the air after reaching the highest jump point
    public float hangTime = .2f;
    // if the player is hang timing
    public bool hangTiming = false;
    public bool ascendCoroutineRunning;
    public bool hangTimeCoroutineRunning;
    // bounce force to apply when bouncing on top of an enemy
    public float bounceForce;
    public float bouncingTime = 0.1f;
    public bool bouncing = false;
    private Coroutine hangTimeCoroutine;
    // coyote time
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [Header("Ground Check")]
    // reference to the transform of the ground check
    public Transform groundCheck;
    // layers in which the player considers being grounded
    public LayerMask groundLayer;
    // defining the verification area of contact with the ground
    public Vector2 sizeGroundCheck = new Vector2(0.16f, 0.02f);
    public bool grounded = false;

    [Header("Dash")]
    // boolean for tracking if the player has finished dashing or not, not taking into account the cooldown
    public bool dashing = false;
    public float dashingPower = 24f;
    public float dashingDuration = 0.2f;
    public float dashingCooldown = 1f;
    public float waitTimeAfterDashing = 0.1f;
    // boolean to see if the cooldown is over
    public bool dashCooldownFinished = true;
    public bool canDoubleDash = false;
    public Coroutine dashingCoroutine;
    public bool inBlockRange = false;
    public bool blockCanBeMoved = false;

    [Header("Interaction")]
    public bool canInteract = false;
    public bool chestOpened = false;

    [Header("Feedback")]
    // Reference to the player's sprite renderer to apply the color
    public SpriteRenderer spriteRenderer;
    // Coroutine type variable, to manage the color flash coroutine
    private Coroutine colorFlashCoroutine;
    public Transform jumpEffectSpawnPoint;
    // to keep track if in the previous frame the player was grounded or not
    private float previousSpeed = 0f;
    public Transform dashEffectSpawnPointLeft;
    public Transform dashEffectSpawnPointRight;
    public Transform respawnSpawnPoint;

    [Header("GoldenCarrot")]
    // duration of the effect of the power up
    public float invencibilityDuration;
    // to count the duration of the power up
    public float invencibilityCounter = 0f;
    // speed boost while on invencibility
    public float speedBoost = 1f;
    public float speedBoostMagnitude = 1.1f;
    // reference to the particle system to display
    //public ParticleSystem powerUpParticle;
    private Coroutine invencibilityCoroutine;
    public bool isInvencible = false;

    [Header("SnailEffect")]
    public float snailEffectDuration = 5f;
    public float speedReductionMagnitude = 0.8f;
    // variable to apply the speed reduction
    public float speedReduction = 1f;
    public float speedReductionCounter;

    [Header("BeeEffect")]
    public float beeEffectDuration = 3f;
    public float jumpReductionMagnitude = 0.8f;
    // variable to apply the jump reduction
    public float jumpReduction = 1f;
    public float jumpReductionCounter;

    // reference to the animator
    public Animator animator;

    public static PlayerController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Method for assigning components
    /// </summary>
    [ContextMenu("FillComponents")]
    public void FillComponents() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Movement();
        EvaluateGrounded();
        EvaluateFalling();
        CheckCanDoubleDash();
        FeedbackOnFall();
        CheckIsInvencible();
        FeedAnimation();
    }

    private void LateUpdate() {
        RestrictVelocity();
        // update the previous grounded variable at the end of the frame
        previousSpeed = rigidBody.velocity.y;
    }

    private void OnDrawGizmos() {
        // color of the cube
        Gizmos.color = Color.red;
        // drawing a cube with the dimensions defined for the overlap box used for the ground check
        Gizmos.DrawWireCube(groundCheck.position, sizeGroundCheck);
    }

    /// <summary>
    /// Verify if the player is touching the ground or not
    /// </summary>
    private void EvaluateGrounded() {
        // checking contact with the ground using overlapbox, indicating its position, size and layer to be used
        grounded = Physics2D.OverlapBox(groundCheck.position, sizeGroundCheck, 0f, groundLayer);

        // as we procedurally generate the level, at the time of unifying colliders they may be small gaps between colliders that make the player get stuck.
        // For this reason, if we are grounded we "deactivate" the gravity so we don't fall into the small gaps
        if (grounded) {
            coyoteTimeCounter = coyoteTime;
            rigidBody.gravityScale = 0;
            falling = false;
            hangTiming = false;
            jumped = false;
        } else {
            // decrease the coyote time counter
            coyoteTimeCounter -= Time.deltaTime;
            // if we are not grounded and not falling set the gravity scale to 1
            if (!falling && !dashing) rigidBody.gravityScale = 1;
        }
    }

    /// <summary>
    /// Moves the player when pressing the movement keys/buttons
    /// </summary>
    public void Movement()
    {
        if (!canMove || dashing || bouncing) return;
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        rigidBody.velocity = new Vector2(horizontalInput * moveSpeed * speedReduction * speedBoost, rigidBody.velocity.y);

        // flip the sprite in the X axis if the player moves to the left
        FlipSprite(horizontalInput);
        // if the velocity is 0 the player is not moving, if is negative the player is moving to the left and if is positive the player is moving to the right
        moving = horizontalInput != 0 ? true : false;
    }

    /// <summary>
    /// Flips the sprite and applies an offest
    /// </summary>
    /// <param name="horizontalInput"></param>
    private void FlipSprite(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            // when we are going to the left, flip the sprite and slightly move the sprite locally to the left
            spriteRenderer.flipX = true;
            spriteRenderer.transform.localPosition = new Vector3(-spriteOffset, spriteRenderer.transform.localPosition.y, 0);
        }
        else if (horizontalInput > 0)
        {
            // when we are going to the left, leave the sprite as it is and slightly move the sprite locally to the right
            spriteRenderer.flipX = false;
            spriteRenderer.transform.localPosition = new Vector3(spriteOffset, spriteRenderer.transform.localPosition.y, 0);
        }
    }

    /// <summary>
    /// Defining the event that reacts to the jumping action
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) Jump();
    }

    /// <summary>
    /// Applies jump force
    /// </summary>
    public void Jump() {
        // if the player is not grounded return and don't jump
        if (coyoteTimeCounter <= 0 || dashing || !canMove) return;

        falling = false;
        coyoteTimeCounter = 0f;

        // reset the current velocity, so the jump is always the same
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);

        jumped = true;

        // start the jumping coroutine
        StartCoroutine(StartJump());

        // only show the jumping particles if the player is jumping
        if (!bouncing) PlayerFeedback.instance.ShowJumpParticles(jumpEffectSpawnPoint.position, transform.rotation);
    }

    /// <summary>
    /// Method to count the time that the player spends ascending before reaching the peak
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartJump()
    {
        ascendCoroutineRunning = true;

        // applying a vertical impulse upwards
        rigidBody.AddForce(Vector2.up * jumpForce * jumpReduction, ForceMode2D.Impulse);

        grounded = false;

        ascendCounter = ascendTime;

        // keep increasing the gravity scale while the ascending time is on
        while (ascendCounter > 0)
        {
            if (dashing) break;
            rigidBody.gravityScale *= 1.01f;
            ascendCounter -= Time.deltaTime;
            yield return null;
        }
        ascendCoroutineRunning = false;

        // start the hang time method when we reach the upper limit
        EvaluateHangJump();
    }

    /// <summary>
    /// Starts the hang time coroutine when the player reaches the upper point
    /// </summary>
    public void EvaluateHangJump() {
        if (grounded || rigidBody.velocity.y < 0) return;

        // if we finished ascending
        if (!ascendCoroutineRunning) {
            // start the Hang Time coroutine
            hangTimeCoroutine = StartCoroutine(HangTime());
        }
    }

    /// <summary>
    /// Coroutine for performing the hang time in the peak of the jump.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HangTime()
    {
        hangTimeCoroutineRunning = true;
        hangTiming = true;
        // stop completely the rigid body's velocity in the y axis and set the gravity scale to 0
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.gravityScale = 0;
        // wait for .2 seconds
        yield return new WaitForSeconds(hangTime);
        // restore the gravity scale
        rigidBody.gravityScale = 1;
        hangTimeCoroutineRunning = false;
    }

    /// <summary>
    /// Method for evaluating if the player is falling after jumping
    /// </summary>
    public void EvaluateFalling() {
        // if the velocity in the y axis is negative means we are falling
        // check if the velocity is less than the maximum falling speed
        if (rigidBody.velocity.y >= maxFallSpeed
            && rigidBody.velocity.y < 0
            && !hangTimeCoroutineRunning
            && !bouncing
            && !dashing) {

            falling = true;
        }
    }

    /// <summary>
    /// If the player's velocity goes off the limit of the defined max speed, set the velocity to the max speed
    /// </summary>
    public void RestrictVelocity()
    {
        if (rigidBody.velocity.y <= maxFallSpeed)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
        }
    }

    /// <summary>
    /// Checks if the player was now grounded in the previous frame and if it is in this one, if so, instantiates the jump particles
    /// </summary>
    public void FeedbackOnFall() {
        if (previousSpeed < 0 && grounded && Mathf.Abs(rigidBody.velocity.y) <= 0.001f) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            PlayerFeedback.instance.ShowJumpParticles(jumpEffectSpawnPoint.position, transform.rotation);
        }
    }

    /// <summary>
    /// Changes the player's velocity on the y axis when bouncing on an enemy
    /// </summary>
    public void BounceOnEnemy() {
        coyoteTimeCounter = coyoteTime;
        dashing = false;
        bouncing = true;
        falling = false;
        grounded = true;
        rigidBody.gravityScale = 1f;
        float originalForce = jumpForce;
        jumpForce += 0.5f;
        Jump();
        jumpForce = originalForce;
        bouncing = false;
    }

    /// <summary>
    /// Defining the event that reacts to the dashing action
    /// </summary>
    /// <param name="context"></param>
    public void OnDash(InputAction.CallbackContext context) {
        if (context.started) Dash();
    }

    /// <summary>
    /// Checks if the dashing cooldown is over and if it is, calls the Dashing coroutine
    /// </summary>
    public void Dash() {
        if (!dashCooldownFinished || dashing || !canMove) return;
        if (hangTimeCoroutineRunning && hangTimeCoroutine != null) StopCoroutine(hangTimeCoroutine);
        dashingCoroutine = StartCoroutine(PerformDash());
    }

    /// <summary>
    /// Coroutine for performing the dash
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformDash() {
        dashing = true;
        falling = false;

        // store the gravity scale before dashing and set the gravity scale to 0
        float startGravity = rigidBody.gravityScale;

        // if the sprite's local position in x is 0 means that we just started the game and the player hasn't moved yet
        float dashingDirection = spriteRenderer.transform.localPosition.x == 0 ? 1 : (spriteRenderer.transform.localPosition.x / spriteOffset);

        

        // deactivate the gravity scale
        rigidBody.gravityScale = 0;

        // 0 in the y axis as we want the dashing to be straight
        rigidBody.velocity = new Vector2(dashingPower * dashingDirection, 0f);

        if (dashingDirection == 1 && inBlockRange) blockCanBeMoved = true;

        // if the dashing direction is negative show the inverted dashing effect version, otherwise show the regular one in the corresponding position
        PlayerFeedback.instance.ShowDashParticles(dashingDirection > 0 ? dashEffectSpawnPointLeft.position : dashEffectSpawnPointRight.position,
                                                transform.rotation,
                                                dashingDirection < 0 ? true : false);

        yield return new WaitForSeconds(dashingDuration);

        //rigidBody.velocity = new Vector2(dashingDirection * 0.1f, 0f);

        //yield return new WaitForSeconds(waitTimeAfterDashing);

        if(!grounded) EvaluateHangJump();

        rigidBody.gravityScale = startGravity;

        dashing = false;
        
        StartCoroutine(DashCooldown(dashingCooldown));
    }

    /// <summary>
    /// Allows the double dashing option
    /// </summary>
    public void AllowDoubleDash() {
        canDoubleDash = true;
    }

    /// <summary>
    /// Coroutine for waiting the given seconds as cooldown. Resets the cooldown if a carrot has been picked
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator DashCooldown(float seconds) {
        dashCooldownFinished = false;
        float counter = seconds;

        while (counter > 0) {
            // if the player can dash while on cooldown, break from the coroutine
            if (canDoubleDash) {
                dashCooldownFinished = true;
                canDoubleDash = false;
                yield break;
            }
            counter -= Time.deltaTime;
            yield return null;
        }
        dashCooldownFinished = true;
    }

    /// <summary>
    /// Checks if the player can double dash and if not, changes the UI image of the score
    /// </summary>
    public void CheckCanDoubleDash() {
        if (!canDoubleDash && UIController.instance.scoreCarrot.enabled) UIController.instance.ChangeScoreSprite();
    }

    /// <summary>
    /// Activates the invencibility gained when picking up the golden carrot
    /// </summary>
    /// <param name="duration"></param>
    public void ActivateInvencibility(float duration) {
        // if the coroutine exists, stop it
        if (invencibilityCoroutine != null) StopCoroutine(invencibilityCoroutine);
        PlayerFeedback.instance.ShowGoldenCarrotFeedback(duration);
        invencibilityCoroutine = StartCoroutine(InvencibilityDuration(duration));
    }

    /// <summary>
    /// Coroutine to count the duration of the invencibility
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator InvencibilityDuration(float duration) {
        // initialize the time counter
        invencibilityCounter = duration;

        float startingSpeed = speedBoost;
        // increment the speed while on invencibility
        speedBoost = speedBoostMagnitude;

        // remove the speed reduction
        speedReduction = 1;

        isInvencible = true;

        while (invencibilityCounter > 0) {
            invencibilityCounter -= Time.deltaTime;
            yield return null;
        }

        isInvencible = false;
        // go back to the base speed
        speedBoost = startingSpeed;
    }

    /// <summary>
    /// Starts the color flashing coroutine through the given time
    /// </summary>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public void StartColorFlash(Color color, float time) {
        // if != null means that the coroutine is running, we need to stop it without waiting for the coroutine to end and then start it again
        if (colorFlashCoroutine != null) StopCoroutine(colorFlashCoroutine);
        // starting the coroutine that will recover the color in the given time
        colorFlashCoroutine = StartCoroutine(ColorChange(color, time));

    }


    /// <summary>
    /// Changes between the original sprite color and the given one through the given time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator ColorChange(Color color, float time) {
        float counter = 0;

        while (counter < time) {
            // rapidly change between the base color and  the given color
            spriteRenderer.color = Color.Lerp(Color.white, color, Mathf.PingPong(counter / Time.deltaTime, 1));
            counter += Time.deltaTime;
            // we wait for the frame to end to update the coroutine
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Activates the snail effect 
    /// </summary>
    /// <param name="activate"></param>
    public void ActivateSnailEffect() {
        // change the moving speed
        speedReduction = speedReductionMagnitude;
        PostProcessingManager.instance.ApplyEnemyScreenEffect(snailEffectDuration, false);
        StartCoroutine(SpeedReductionDuration(snailEffectDuration));
    }

    /// <summary>
    /// Coroutine to count the duration of the speed reduction
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator SpeedReductionDuration(float duration) {
        // initialize the time counter
        speedReductionCounter = duration;

        while (speedReductionCounter > 0) {
            speedReductionCounter -= Time.deltaTime;
            yield return null;
        }
        // reset the original speed
        speedReduction = 1f;
    }

    /// <summary>
    /// Checks if the player is in invencible mode and then removes all negative effects and associated visual effects
    /// </summary>
    public void CheckIsInvencible() {
        if(invencibilityCounter > 0) {
            speedReduction = 1f;
            jumpReduction = 1f;
            PlayerFeedback.instance.RemoveAllNegativeVisualEffects();
        }
    }

    /// <summary>
    /// Defining the event that reacts to the interaction action
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started && canInteract) {
            chestOpened = true;
        }
    }

    /// <summary>
    /// Activates the effect 
    /// </summary>
    public void ActivateBeeEffect() {
        // change the moving speed
        jumpReduction = jumpReductionMagnitude;
        PostProcessingManager.instance.ApplyEnemyScreenEffect(beeEffectDuration, true);
        StartCoroutine(JumpReductionDuration(beeEffectDuration));
    }

    /// <summary>
    /// Coroutine to count the duration of the jump reduction
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator JumpReductionDuration(float duration) {
        // initialize the time counter
        jumpReductionCounter = duration;

        while (jumpReductionCounter > 0) {
            jumpReductionCounter -= Time.deltaTime;
            yield return null;
        }
        // reset the original speed
        jumpReduction = 1f;
    }

    /// <summary>
    /// Freezes the player's movement
    /// </summary>
    public void FreezeMovement() {
        canMove = false;
        moving = false;
        dashing = false;
        rigidBody.velocity = Vector3.zero;
        FlipSprite(-1);
    }

    /// <summary>
    /// Un freezes the movement allowing the player to move again
    /// </summary>
    public void UnFreezeMovement() {
        canMove = true;
    }

    /// <summary>
    /// Starts the coroutine of the player's behaviour in the last part of the tutorial
    /// </summary>
    public void MovePlayerInTutorial() {
        if (canMove) return;
        StartCoroutine(SurprisePlayerAndWait());
    }

    /// <summary>
    /// Coroutine to change the animation of the player and automatically moving it to the right
    /// </summary>
    /// <returns></returns>
    private IEnumerator SurprisePlayerAndWait() {
        animator.SetBool(Constants.ANIM_PLAYER_SURPRISED, true);
        yield return new WaitForSeconds(1f);
        animator.SetBool(Constants.ANIM_PLAYER_SURPRISED, false);

        moving = true;

        speedReduction = 0.7f;

        rigidBody.velocity = new Vector2(moveSpeed * speedReduction, rigidBody.velocity.y);

        // flip the sprite in the X axis if the player moves to the left
        FlipSprite(1);
    }

    /// <summary>
    /// Updates the values of the variables in the animator that require constant update
    /// </summary>
    private void FeedAnimation() {
        animator.SetBool(Constants.ANIM_PLAYER_MOVING, moving);
        animator.SetBool(Constants.ANIM_PLAYER_GROUNDED, grounded);
        animator.SetBool(Constants.ANIM_PLAYER_HANG_TIMING, hangTiming);
        animator.SetBool(Constants.ANIM_PLAYER_FALLING, falling);
        animator.SetFloat(Constants.ANIM_PLAYER_VELOCITY, Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool(Constants.ANIM_PLAYER_DASHING, dashing);
    }
}