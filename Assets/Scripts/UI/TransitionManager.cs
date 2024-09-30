using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public float fadeDuration = 5f;
    public Image fadeImage;
    public Gradient fadeColorGradient;
    private float timeCounter = 0;
    public bool fade = false;

    public static TransitionManager instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
        if(SceneManager.GetActiveScene().name.Equals(Constants.TUTORIAL_SCENE)) MusicManager.instance.PlayTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        FadeScreen();
        ShowTutorialCompletedPanel();
    }

    /// <summary>
    /// Fades the screen into a black screen
    /// </summary>
    public void FadeScreen() {
        if (!fade) return;
        fadeImage.color = fadeColorGradient.Evaluate(timeCounter / fadeDuration);
        timeCounter += Time.deltaTime;
        if (fadeImage.color.a == 1f) fade = false;
    }

    /// <summary>
    /// Shows the tutorial end panel
    /// </summary>
    public void ShowTutorialCompletedPanel() {
        if (fadeImage.color.a != 1f) return;
        UIController.instance.EnableTutorialCompletedPanel(true);
    }
}
