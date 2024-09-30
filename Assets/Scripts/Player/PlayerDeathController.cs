using System.Collections;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    public bool dead = false;
    public Rigidbody2D rigidBody;
    public Collider2D collider;
    public float impulseForce = 2f;

    // reference to the animator
    public Animator animator;

    public static PlayerDeathController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    [ContextMenu("FillComponents")]
    private void FillComponents() {
        collider = GetComponentInChildren<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FeedAnimator();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (((collision.CompareTag(Constants.TAG_GHOST) && !PlayerController.instance.isInvencible)) 
            || collision.CompareTag(Constants.TAG_DEATH_LIMIT)) {
            GhostController.instance.StopMovement();
            KillPlayer();
        }
    }

    /// <summary>
    /// Freezes the player's movement and starts the killing coroutine
    /// </summary>
    public void KillPlayer() {
        PlayerController.instance.FreezeMovement();
        StartCoroutine(KillPlayerCoroutine());
    }

    /// <summary>
    /// Coroutine for waiting a short amount of time before killing the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator KillPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        dead = true;
        DeathImpulseAnimation();
        GameManager.instance.CheckEndGame();
    }

    /// <summary>
    /// Applies a vertical impulse when the player dies and deactivates the collider so it falls down
    /// </summary>
    public void DeathImpulseAnimation() {
        rigidBody.gravityScale = 1f;
        rigidBody.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
        collider.enabled = false;
    }

    public void FeedAnimator() {
        animator.SetBool(Constants.ANIM_PLAYER_DEAD, dead);
    }
}
