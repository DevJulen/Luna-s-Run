using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    [Header("Value")]
    // value of the pickup, if the pickup is the star the value will be 1
    // in the case of the carrot, this variable will not be taken into account
    // in the case of the golden carrot, this variable will store the duration of the invencibility
    public int collectableValue = 1;
    // defining the pickup type as the star by default
    public Enums.PickupType type = Enums.PickupType.STAR;

    public Collider2D collider;
    public SpriteRenderer spriteRenderer;

    [Header("Feedback")]
    public Color flashColor = Color.white;
    public float flashTime = 0.4f;
    public GameObject pickUpEffect;
    public AudioSource audioSource;
    public AudioClip feedbackSound;
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    public static Pickup instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Method for assigning components
    /// </summary>
    [ContextMenu("FillComponents")]
    public void FillComponents()
    {
        //audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // configure audio source to not play on awake
        //audioSource.playOnAwake = false;
        // configure audio source to not loop
        //audioSource.loop = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.TAG_PLAYER))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (type)
            {
                case Enums.PickupType.STAR:
                    // call the game manager to pickup the star with a value of 1
                    if (SceneManager.GetActiveScene().name.Equals(Constants.LOAD_SCREEN_SCENE)) break;
                    GameManager.instance.PickupStar(collectableValue);
                    if (SceneManager.GetActiveScene().name.Equals(Constants.TUTORIAL_SCENE)) GameManager.instance.pickedStarsInTutorialSection++;
                    break;
                case Enums.PickupType.CARROT:
                    if (player != null) {
                        //Feedback(collision.gameObject);
                        UIController.instance.ChangeScoreSprite();
                        player.AllowDoubleDash();
                    }
                    break;
                case Enums.PickupType.GOLDEN_CARROT:
                    if (player != null) {
                        //Feedback(collision.gameObject);
                        // activate the invencibility on the player
                        player.ActivateInvencibility(collectableValue);
                    }
                    break;
                case Enums.PickupType.PURPLE_STAR:
                    if (SceneManager.GetActiveScene().name.Equals(Constants.LOAD_SCREEN_SCENE)) break;
                    // call the game manager to pickup the star with a value of 5
                    GameManager.instance.PickupStar(collectableValue);
                    break;
            }
            if (!SceneManager.GetActiveScene().name.Equals(Constants.LOAD_SCREEN_SCENE)) {
                // adding a little variation to the star picking sound each time the player picks up one
                if (type == Enums.PickupType.STAR || type == Enums.PickupType.PURPLE_STAR) {
                    audioSource.pitch = Random.Range(pitchMin, pitchMax);
                }
                audioSource.clip = feedbackSound;
                audioSource.volume = 0.2f;
                audioSource.Play();
            }
            DeactivateCollectable();
            StartCoroutine(SpawnAndDeactivateAnimation());
        }
    }

    private IEnumerator SpawnAndDeactivateAnimation() {
        GameObject effect = Instantiate(pickUpEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(effect);
    }

    /// <summary>
    /// Method for deactivating the pick up interactively and visually
    /// </summary>
    private void DeactivateCollectable()
    {
        // deactivate all possible interaction between the player and the collectable
        collider.enabled = false;
        // deactivate the visual element of the collectable
        spriteRenderer.enabled = false;
    }
}
