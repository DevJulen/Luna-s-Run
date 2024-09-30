using UnityEngine;


public class GhostTutorialController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool move = false;

    public Transform movePoint;

    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    public GameObject rightLimit;

    public Animator animator;

    public static GhostTutorialController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Update() {
        GhostBehaviour();
    }

    [ContextMenu("FillComponents")]
    public void FillComponents() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Defines the enemy ghost's behaviour in the tutorial's last section
    /// </summary>
    public void GhostBehaviour() {
        if (!move) return;

        spriteRenderer.color = new Color(255f, 255f, 255f, 0.9f);

        transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, moveSpeed * Time.deltaTime);
        if (transform.position.x >= movePoint.position.x) {
            move = false;
            TutorialCameraController.instance.ShakeCamera();
            rightLimit.GetComponent<Collider2D>().isTrigger = true;
            animator.SetBool(Constants.ANIM_GHOST_SCREAM, true);
            MusicManager.instance.PlayGame(true);
            TransitionManager.instance.fade = true;
            PlayerController.instance.MovePlayerInTutorial();
        }
    }
}
