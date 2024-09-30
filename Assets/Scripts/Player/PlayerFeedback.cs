using System.Collections;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    private Coroutine colorFlashCoroutine;
    private Coroutine colorRecoverCoroutine;

    [Header("GoldenCarrot")]
    public Color goldenCarrotColor;

    [Header("JumpEffect")]
    public GameObject jumpEffect;
    public float jumpParticleTime = 0.5f;
    public AudioClip jumpClip;

    [Header("DashEffect")]
    public GameObject dashEffect;
    public GameObject dashEffectInverted;
    public float dashParticleTime = 0.5f;
    public AudioClip dashClip;

    [Header("EnemyDeathEffect")]
    public GameObject deathEffect;
    public float deathEffectTime = 0.5f;
    public AudioClip enemyDeathEffect;

    [Header("RespawnEffect")]
    public GameObject respawnEffect;
    public float respawnEffectTime = 0.5f;

    public AudioSource audioSource;

    public static PlayerFeedback instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
        playerSprite = GetComponentInChildren<SpriteRenderer>();    
    }

    /// <summary>
    /// Instantiates the dashing effect in the given position and inverted if we want
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <param name="inverted"></param>
    public void ShowDashParticles(Vector3 spawnPoint, Quaternion rotation, bool inverted) {
        StartCoroutine(DashParticlesCoroutine(spawnPoint, rotation, inverted));
    }

    /// <summary>
    /// Coroutine for instantiating and destroying the dashing effect after the given time
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <param name="inverted"></param>
    /// <returns></returns>
    public IEnumerator DashParticlesCoroutine(Vector3 spawnPoint, Quaternion rotation, bool inverted) {
        audioSource.clip = dashClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
        GameObject effectClone = Instantiate(inverted ? dashEffectInverted : dashEffect, spawnPoint, rotation);
        yield return new WaitForSeconds(dashParticleTime);
        Destroy(effectClone);
    }

    /// <summary>
    /// Instantiates the jump effect in the given position
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    public void ShowJumpParticles(Vector3 spawnPoint, Quaternion rotation) {
        StartCoroutine(JumpParticlesCoroutine(spawnPoint, rotation));
    }

    /// <summary>
    /// Coroutine for instantiating and destroying the jumping effect after the given time
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public IEnumerator JumpParticlesCoroutine(Vector3 spawnPoint, Quaternion rotation) {
        
        if (PlayerController.instance.jumped && !PlayerController.instance.falling) {
            audioSource.clip = jumpClip;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
        GameObject effectClone = Instantiate(jumpEffect, spawnPoint, rotation);
        yield return new WaitForSeconds(jumpParticleTime);
        Destroy(effectClone);
    }

    /// <summary>
    /// Instantiates the death effect in the given position
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    public void ShowDeathEffect(Vector3 spawnPoint, Quaternion rotation)
    {
        StartCoroutine(DeathEffectCoroutine(spawnPoint, rotation));
    }

    /// <summary>
    /// Coroutine for instantiating and destroying the death effect after the given time
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public IEnumerator DeathEffectCoroutine(Vector3 spawnPoint, Quaternion rotation)
    {
        audioSource.clip = enemyDeathEffect;
        audioSource.volume = 0.2f;
        audioSource.Play();
        GameObject effectClone = Instantiate(deathEffect, spawnPoint, rotation);
        yield return new WaitForSeconds(deathEffectTime);
        Destroy(effectClone);
    }

    /// <summary>
    /// Instantiates the dashing effect in the given position and inverted if we want
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <param name="inverted"></param>
    public void ShowRespawnEffect(Vector3 spawnPoint, Quaternion rotation) {
        StartCoroutine(RespawnEffectCoroutine(spawnPoint, rotation));
    }

    /// <summary>
    /// Coroutine for instantiating and destroying the dashing effect after the given time
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="rotation"></param>
    /// <param name="inverted"></param>
    /// <returns></returns>
    public IEnumerator RespawnEffectCoroutine(Vector3 spawnPoint, Quaternion rotation) {
        GameObject effectClone = Instantiate(respawnEffect, spawnPoint, rotation);
        yield return new WaitForSeconds(respawnEffectTime);
        Destroy(effectClone);
    }

    /// <summary>
    /// Starts the color flash with the golden carrot's color
    /// </summary>
    /// <param name="time"></param>
    public void ShowGoldenCarrotFeedback(float time) {
        StartColorFlash(goldenCarrotColor, time, true);
    }

    /// <summary>
    /// Assigns the flash color and starts the color recover coroutine for the given time
    /// </summary>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public void StartColorFlash(Color color, float time, bool goldenCarrot) {
        playerSprite.color = color;

        if(!goldenCarrot) {
            if (colorFlashCoroutine != null) StopCoroutine(colorFlashCoroutine);
            colorFlashCoroutine = StartCoroutine(ColorRecover(time));
        } else {
            if (colorRecoverCoroutine != null) StopCoroutine(colorRecoverCoroutine);
            colorRecoverCoroutine = StartCoroutine(ColorChange(time));
        }
        
    }


    /// <summary>
    /// Recovers the original color in the given time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator ColorRecover(float time) {
        float counter = 0;
        // store the initial color value before the Lerp
        Color startColor = playerSprite.color;

        while (counter < time) {
            playerSprite.color = Color.Lerp(startColor, Color.white, counter / time);
            counter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Changes between the original sprite color and the given one through the given time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator ColorChange(float time) {
        float counter = 0;

        while (counter < time) {
            // rapidly change between the base color and  the given color
            //playerSprite.color = Color.Lerp(Color.white, goldenCarrotColor, Mathf.PingPong(counter / Time.deltaTime, 1));
            playerSprite.color = new Color(Random.value, Random.value, Random.value, 0.9f);
            counter += Time.deltaTime;
            // we wait for the frame to end to update the coroutine
            yield return new WaitForEndOfFrame();
        }
        playerSprite.color = Color.white;
    }

    /// <summary>
    /// Restores the player's sprite color instantly and calls to remove all post processing effects
    /// </summary>
    public void RemoveAllNegativeVisualEffects() {
        PostProcessingManager.instance.RemoveEffects();
        //playerSprite.color = Color.white;
    }
}
