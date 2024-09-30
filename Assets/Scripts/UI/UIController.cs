using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Score")]
    public CanvasGroup gui;
    public Image scoreNoCarrot;
    public Image scoreCarrot;

    public TMP_Text starCount;

    [Header("PauseMenu")]
    public CanvasGroup pauseMenu;
    public bool canPause;

    [Header("ContinuePanel")]
    public CanvasGroup continuePanel;

    [Header("EndGame")]
    public CanvasGroup endGameCanvas;
    public TMP_Text snailsKilledText;
    public TMP_Text beesKilledText;
    public TMP_Text finalStarCountText;

    [Header("TutorialEnd")]
    public CanvasGroup tutorialEndPanel;

    public static UIController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        canPause = true;
        if(gui != null) gui.alpha = 1f;
        starCount.text = GameManager.instance.starCount.ToString();
    }

    /// <summary>
    /// Updates the star count in the UI
    /// </summary>
    /// <param name="count"></param>
    public void UpdateStarCountText(int count) {
        starCount.text = count.ToString();
    }

    /// <summary>
    /// Changes the sprite of the panel containing the score between the one with the carrot and the one without it
    /// </summary>
    public void ChangeScoreSprite() {
        if(!scoreNoCarrot.enabled) {
            scoreNoCarrot.enabled = true;
            scoreCarrot.enabled = false;
        } else {
            scoreNoCarrot.enabled = false;
            scoreCarrot.enabled = true; ;
        }
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause(bool paused) {
        if (!canPause) return;
        // if the player wants to pause, stop time
        Time.timeScale = paused ? 0f : 1f;

        gui.alpha = paused ? 0f : 1f;

        PlayerController.instance.canMove = !paused;

        // enable or disable the pause menu depending on the paused variable
        EnableCanvasGroup(pauseMenu, paused);
    }

    /// <summary>
    /// Enables the end game panel with information about the game
    /// </summary>
    public void ShowEndGamePanel() {
        MusicManager.instance.PlayDeathMusic();
        snailsKilledText.text = GameManager.instance.snailsKilled.ToString();
        beesKilledText.text = GameManager.instance.beesKilled.ToString();
        finalStarCountText.text = GameManager.instance.starCount.ToString();

        gui.alpha = 0f;
        EnableCanvasGroup(endGameCanvas, true);
    }

    /// <summary>
    /// Enables or disables the continue panel and the score elementss
    /// </summary>
    /// <param name="enable"></param>
    public void EnableContinuePanel(bool enable) {
        EnableGUI(!enable);
        EnableCanvasGroup(continuePanel, enable);
    }

    /// <summary>
    /// Enables or disables the tutorial completed panel and the score elements
    /// </summary>
    /// <param name="enable"></param>
    public void EnableTutorialCompletedPanel(bool enable) {
        EnableGUI(!enable);
        EnableCanvasGroup(tutorialEndPanel, enable);
    }

    /// <summary>
    /// Enables or disables the score elements
    /// </summary>
    /// <param name="enable"></param>
    public void EnableGUI(bool enable) {
        gui.alpha = enable ? 1f : 0f;
    }

    /// <summary>
    /// Hides or enables the given canvas group
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="enable"></param>
    public void EnableCanvasGroup(CanvasGroup canvasGroup, bool enable) {
        PostProcessingManager.instance.ActivateDepthOfField(enable);
        // making the canvas visible or invisible
        canvasGroup.alpha = enable ? 1 : 0;
        // making the canvas interactable or not
        canvasGroup.interactable = enable;
        // making the canvas block raycasts or not
        canvasGroup.blocksRaycasts = enable;
    }
}
