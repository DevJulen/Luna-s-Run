using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ChestController : MonoBehaviour
{
    public SpriteRenderer closedSpriteRenderer;
    public SpriteRenderer openSpriteRenderer;

    [Header("Loot")]
    // how many stars does the chest contain
    public int lootAmount = 5;
    public GameObject loot;
    // speed at which the loot moves from the chest
    public float lootMoveSpeed = 0.3f;
    private bool hasBeenOpened = false;
    public bool canBeOpened = false;

    private GameObject starClone;
    private Vector3 starSpawnPoint;
    private Vector2 startingPoint;
    public Vector3 spawnPointOffset = new Vector3(0f, 0.07f, 0f);

    [Header("Effects")]
    public GameObject enchantedEffect;
    public Transform enchantedEffectPoint;
    public GameObject enchantedEffectClone;
    public AudioClip openClip;

    public GameObject smokeEffect;
    public Transform smokeEffectPoint;

    [Header("InteractKeyButton")]
    public GameObject keyboardKey;
    public GameObject xboxButton;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // if we are in the tutorial scene instantiate the enchanted effect
        if (SceneManager.GetActiveScene().name.Equals(Constants.TUTORIAL_SCENE))  enchantedEffectClone = Instantiate(enchantedEffect, enchantedEffectPoint.position, enchantedEffect.transform.rotation);
        keyboardKey.SetActive(false);
        xboxButton.SetActive(false);
        closedSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        openSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        starSpawnPoint = transform.position + spawnPointOffset;
        startingPoint = transform.position;
    }

    private void Update() {
        GiveLoot();
    }

    /// <summary>
    /// Spawns a little sprite for telling the player the key or button that has to press in order to open the chest
    /// </summary>
    /// <param name="spawn"></param>
    public void SpawnKeyButton(bool spawn)
    {
        // if we exit the chest area despawn both
        if(!spawn) {
            xboxButton.gameObject.SetActive(spawn);
            keyboardKey.gameObject.SetActive(spawn);
        } else { // else, decide which to spawn
            if (GameManager.instance.lastPressed == Constants.KEYBOARD_MOUSE) {
                xboxButton.gameObject.SetActive(!spawn);
                keyboardKey.gameObject.SetActive(spawn);
            } else if (GameManager.instance.lastPressed == Constants.GAMEPAD) {
                keyboardKey.gameObject.SetActive(!spawn);
                xboxButton.gameObject.SetActive(spawn);
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // on trigger enter we set that the player can interact with the chest
        if (collision.CompareTag(Constants.TAG_PLAYER))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !hasBeenOpened) {
                SpawnKeyButton(true);
                player.canInteract = true;
                canBeOpened = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // on trigger exit we set that the player cannot interact with the chest anymore
        if (collision.CompareTag(Constants.TAG_PLAYER))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null) {
                SpawnKeyButton(false);
                player.canInteract = false;
                canBeOpened = false;
            }
        }
    }

    /// <summary>
    /// Changes the chest's sprite, instantiates the loot and starts the coroutine to get the loot out of the chest
    /// </summary>
    public void GiveLoot()
    {
        if (!(PlayerController.instance.chestOpened && canBeOpened && PlayerController.instance.canInteract)) return;

        PlayerController.instance.canInteract = false;
        PlayerController.instance.chestOpened = false;
        canBeOpened = false;

        // if the chest is located in the tutorial scene, shake the screen
        if (SceneManager.GetActiveScene().name.Equals(Constants.TUTORIAL_SCENE)) {
            StartCoroutine(ShowSmokeEffect());
            GhostTutorialController.instance.move = true;
            PlayerController.instance.FreezeMovement();
        }
        xboxButton.gameObject.SetActive(false);
        keyboardKey.gameObject.SetActive(false);

        audioSource.clip = openClip;
        audioSource.volume = 0.3f;
        audioSource.Play();

        // change the sprite
        closedSpriteRenderer.enabled = false;
        openSpriteRenderer.enabled = true;
        // set it to opened state
        hasBeenOpened = true;
        // instantiate the loot
        starClone = Instantiate(loot, startingPoint, transform.rotation, transform);
        // deactivate the collider so the player can only pick it up when it reaches the upper point
        starClone.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(MoveLoot());
    }

    /// <summary>
    /// Coroutine to move the loot out of the chest slowly
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveLoot()
    {
        float counter = lootMoveSpeed;

        while(counter > 0)
        {
            starClone.transform.position = Vector3.MoveTowards(starClone.transform.position, starSpawnPoint, lootMoveSpeed * Time.deltaTime);
            counter -= Time.deltaTime;
            yield return null;
        }
        // reactivate the collider to allow the player to pick it up
        starClone.GetComponent<CircleCollider2D>().enabled = true;
    }

    /// <summary>
    /// Destroys the enchanted effect and instantiates and destroys the smoke effect. Only in the tutorial.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowSmokeEffect() {
        Destroy(enchantedEffectClone);
        GameObject clone = Instantiate(smokeEffect, smokeEffectPoint.position, smokeEffect.transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(clone);
    }
}
