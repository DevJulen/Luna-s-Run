using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PostProcessingManager : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public ChromaticAberration chromaticAberration;
    public DepthOfField depthOfField;
    public Bloom bloom;
    public float maxIntensity = 0.5f;

    private Coroutine enemyEffectCoroutine;

    public Color beeEffectColor;
    public Color snailEffectColor;

    public static PostProcessingManager instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out depthOfField);
        postProcessVolume.profile.TryGetSettings(out bloom);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals(Constants.RANKING_SCENE)) bloom.intensity.value = 0f;
    }

    public void ApplyEnemyScreenEffect(float time, bool bee) {
        if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
        enemyEffectCoroutine = StartCoroutine(ApplyEnemyScreenEffectCoroutine(time));
        PlayerFeedback.instance.StartColorFlash(bee ? beeEffectColor : snailEffectColor, time, false);
    }

    public IEnumerator ApplyEnemyScreenEffectCoroutine(float time) {
        float counter = time;

        chromaticAberration.intensity.value = maxIntensity;

        while (counter > 0f) {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, maxIntensity, counter / time);
            counter -= Time.deltaTime;
            yield return null;
        }
        chromaticAberration.intensity.value = 0f;
    }

    /// <summary>
    /// Removes all post processing effects instantly 
    /// </summary>
    public void RemoveEffects() {
        if(enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
        chromaticAberration.intensity.value = 0f;
    }

    public void ActivateDepthOfField(bool activate) {
        depthOfField.focusDistance.value = activate ? 1f : 10f;
    }

}
