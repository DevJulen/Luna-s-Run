using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip menuClip;
    public AudioClip gameClip;
    public AudioClip tutorialClip;
    public AudioClip deathClip;

    public float fadeTime = 1f;

    public static MusicManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        // if another instance exists, to avoid the overlapping between musics the new instance will be destroyed
        else Destroy(gameObject);
        // avoid this instance to be destroyed between scenes, so that only one unique instance will be used on the entire game and
        // any newly created instances will be destroyed
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Reproduces the tutorial music
    /// </summary>
    public void PlayMainMenu() {
        if (audioSource.clip == menuClip) return;

        audioSource.clip = menuClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    /// <summary>
    /// Reproduces the game music
    /// </summary>
    public void PlayGame(bool tutorial) {
        //if (audioSource.clip == gameClip) return;

        if (tutorial) StartCoroutine(FadeAndChangeClip(gameClip, 0.1f));
        else {
            audioSource.clip = gameClip;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Reproduces the tutorial music
    /// </summary>
    public void PlayTutorial() {
        if (audioSource.clip == tutorialClip) return;

        audioSource.clip = tutorialClip;
        audioSource.volume = 0.15f;
        audioSource.Play();
    }

    /// <summary>
    /// Reproduces the death screen music
    /// </summary>
    public void PlayDeathMusic() {
        //if (audioSource.clip == deathClip) return;

        StartCoroutine(FadeAndChangeClip(deathClip, 0.1f));
    }

    /// <summary>
    /// Stops the music playing
    /// </summary>
    public void StopMusic() {
        audioSource.Stop();
    }

    /// <summary>
    /// Changes the clip with a smooth volume change
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    private IEnumerator FadeAndChangeClip(AudioClip clip, float volume) {
        float counter = 0f;

        float startVolume = audioSource.volume;

        while (counter < fadeTime) {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, counter / fadeTime);
            counter += Time.deltaTime;
            yield return null;
        }
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
