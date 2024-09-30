using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Changes the scene to the game scene
    /// </summary>
    public void GoToGame() {
        TransitionController.instance.sceneToGo = Constants.GAME_SCENE;
        ChangeScene(Constants.LOAD_SCREEN_SCENE);
    }

    /// <summary>
    /// Changes the scene to the main menu scene
    /// </summary>
    public void GoToMainMenu() {
        // only show the load screen if we are going from the game or tutorial scenes to the main menu
        if(TransitionController.instance.sceneToGo.Equals(Constants.GAME_SCENE) || TransitionController.instance.sceneToGo.Equals(Constants.TUTORIAL_SCENE)) {
            TransitionController.instance.sceneToGo = Constants.MAIN_MENU_SCENE;
            ChangeScene(Constants.LOAD_SCREEN_SCENE);
        } else ChangeScene(Constants.MAIN_MENU_SCENE);
    }

    /// <summary>
    /// Changes the scene to the tutorial scene showing the advice scene first
    /// </summary>
    public void GoToTutorial() {
        TransitionController.instance.sceneToGo = Constants.TUTORIAL_SCENE;
        ChangeScene(Constants.LOAD_SCREEN_SCENE);
    }

    /// <summary>
    /// Changes the scene to the ranking scene
    /// </summary>
    public void GoToRanking() {
        ChangeScene(Constants.RANKING_SCENE);
    }

    /// <summary>
    /// Changes the scene to the credits scene
    /// </summary>
    public void GoToCredits()
    {
        ChangeScene(Constants.CREDITS_SCENE);
    }

    /// <summary>
    /// Loads the scene with the scene name given as parameter
    /// </summary>
    /// <param name="nextScene"></param>
    public void ChangeScene(string nextScene) {
        // to make sure that no scene changes happen with the time paused
        Time.timeScale = 1;
        // load the specified scene
        SceneManager.LoadScene(nextScene);
    }
}
