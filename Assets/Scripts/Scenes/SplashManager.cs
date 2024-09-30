using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{

    // scene to jump after the splash screen
    public string sceneAfterSplash = Constants.MAIN_MENU_SCENE;
    // duration of the splash
    [Range(0f, 5f)]
    public float splashDuration = 3f;
    public Image fadeImage;
    // gradient to configure the fading effect
    public Gradient fadeColorGradient;
    private float timeCounter = 0;

    private void Awake()
    {
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {
        // go on in time
        timeCounter += Time.deltaTime;
        // to get a value between 0 and 1, at the beginning time counter is 0 and 0/3 is 0, we dont fade
        fadeImage.color = fadeColorGradient.Evaluate(timeCounter / splashDuration);
        // when reaching the splash duration, load the scene after, in this case the main menu
        if (timeCounter >= splashDuration) SceneManager.LoadScene(sceneAfterSplash);
    }
}
