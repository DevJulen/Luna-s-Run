using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextWritter : MonoBehaviour
{
    public string textToShow;
    public TMP_Text loadingText;
    public float timeBetweenCharacters;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.StopMusic();
        StartCoroutine(WriteTextInScreen());
    }

    /// <summary>
    /// Writes three dots in loop 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WriteTextInScreen()
    {
        while(true)
        {
            if(SceneManager.GetActiveScene().name.Equals(Constants.LOAD_SCREEN_SCENE)) loadingText.text = "LOADING";
            else loadingText.text = "";

            yield return new WaitForSeconds(timeBetweenCharacters);
            foreach (char c in textToShow)
            {
                loadingText.text += c;
                yield return new WaitForSeconds(timeBetweenCharacters);
            }
            yield return new WaitForSeconds(timeBetweenCharacters);
            // if the advice scene is active and we finished writing the text break from the loop
            if (SceneManager.GetActiveScene().name.Equals(Constants.ADVICE_SCENE))
            {
                yield return new WaitForSeconds(1.5f-timeBetweenCharacters);
                SceneManager.LoadScene(Constants.LOAD_SCREEN_SCENE);
                yield break;
            }
        }
    }
}
