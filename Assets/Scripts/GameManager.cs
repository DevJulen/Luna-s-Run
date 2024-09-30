using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("HUD")]
    // variable to count the picked collectables
    public int starCount = 0;

    [Header("Input Management")]
    public string lastPressed = "Gamepad";
    public PlayerInput playerInput;

    [Header("Tutorial")]
    public int pickedStarsInTutorialSection = 0;

    [Header("EndGame")]
    public int snailsKilled = 0;
    public int beesKilled = 0;

    [Header("ContinueSystem")]
    // to track that the player has only been able to respawn one time
    public bool alreadyContinued = false;
    public List<Transform> availablePointsToSpawn;
    public int starsToContinue = 25;
    public float invencibilityAfterRespawn = 3f;
    public GameObject leftScreenLimit;
    public float distanceFromLeftSide = 0.5f;

    // to check if the score obtained has been stored as one of the top scores
    public bool stored = false;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.UpdateStarCountText(starCount);
        if(SceneManager.GetActiveScene().name.Equals(Constants.GAME_SCENE)) MusicManager.instance.PlayGame(false);
    }

    private void Update()
    {
        CheckControlScheme();
    }

    /// <summary>
    /// Increments the amount of the picked stars
    /// </summary>
    /// <param name="value"></param>
    public void PickupStar(int starValue)
    {
        starCount += starValue;
        UIController.instance.UpdateStarCountText(starCount);
    }

    /// <summary>
    /// Decreases the amount of the picked stars
    /// </summary>
    /// <param name="value"></param>
    public void RemoveStarCount(int starValue) {
        starCount -= starValue;
        UIController.instance.UpdateStarCountText(starCount);
    }

    /// <summary>
    /// Cgecks the player input to see if the game is being played with a gamepad or keyboard and mouse
    /// </summary>
    public void CheckControlScheme() {
        // con referencia al player input directamente en vez de getcomponent
        if (playerInput.currentControlScheme.Equals(Constants.KEYBOARD_MOUSE)) {
            lastPressed = Constants.KEYBOARD_MOUSE;
            Cursor.visible = true;
        } else if (playerInput.currentControlScheme.Equals(Constants.GAMEPAD)) {
            lastPressed = Constants.GAMEPAD;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Updates the amount of snails or bees killed depending on the parameter
    /// </summary>
    /// <param name="snail"></param>
    public void AddKilledEnemy(bool snail) {
        if (snail) snailsKilled++;
        else beesKilled++;
    }

    /// <summary>
    /// Checks if the player can continue the game, if not ends the game
    /// </summary>
    public void CheckEndGame() {
        UIController.instance.canPause = false;
        if (starCount > starsToContinue && !alreadyContinued) {
            UIController.instance.EnableContinuePanel(true);
        } else EndGame();
    }

    /// <summary>
    /// Stores the score and shows the end game panel 
    /// </summary>
    public void EndGame() {
        CheckScoreIsTop1();
        CheckScoreIsTop2();
        CheckScoreIsTop3();
        UIController.instance.EnableContinuePanel(false);
        UIController.instance.ShowEndGamePanel();
    }

    /// <summary>
    /// Checks if the obtained score is top 1 stores it in the data manager to later save it
    /// </summary>
    /// <returns></returns>
    public bool CheckScoreIsTop1() {
        // if the score has been already scored exit from the method
        if (stored) return false;
        if (starCount > DataManager.instance.maxStarCount1) {
            DataManager.instance.SaveTop1Score(starCount, beesKilled, snailsKilled);
            DataManager.instance.Save(1);
            stored = true;
            return true;
        } else if (starCount == DataManager.instance.maxStarCount1) {
            if (beesKilled > DataManager.instance.maxBeesKilled1) {
                DataManager.instance.SaveTop1Score(starCount, beesKilled, snailsKilled);
                DataManager.instance.Save(1);
                stored = true;
                return true;
            } else if (beesKilled == DataManager.instance.maxBeesKilled1) {
                if (snailsKilled > DataManager.instance.maxSnailsKilled1) {
                    DataManager.instance.SaveTop1Score(starCount, beesKilled, snailsKilled);
                    DataManager.instance.Save(1);
                    stored = true;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the obtained score is top 3 stores it in the data manager to later save it
    /// </summary>
    /// <returns></returns>
    public bool CheckScoreIsTop2() {
        // if the score has been already scored exit from the method
        if (stored) return false;
        if (starCount > DataManager.instance.maxStarCount2) {
            DataManager.instance.SaveTop2Score(starCount, beesKilled, snailsKilled);
            DataManager.instance.Save(2);
            stored = true;
            return true;
        } else if (starCount == DataManager.instance.maxStarCount2) {
            if (beesKilled > DataManager.instance.maxBeesKilled2) {
                DataManager.instance.SaveTop2Score(starCount, beesKilled, snailsKilled);
                DataManager.instance.Save(2);
                stored = true;
                return true;
            } else if (beesKilled == DataManager.instance.maxBeesKilled2) {
                if (snailsKilled > DataManager.instance.maxSnailsKilled2) {
                    DataManager.instance.SaveTop2Score(starCount, beesKilled, snailsKilled);
                    DataManager.instance.Save(2);
                    stored = true;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the obtained score is top 3 stores it in the data manager to later save it
    /// </summary>
    /// <returns></returns>
    public bool CheckScoreIsTop3() {
        // if the score has been already scored exit from the method
        if (stored) return false;
        if (starCount > DataManager.instance.maxStarCount3) {
            DataManager.instance.SaveTop3Score(starCount, beesKilled, snailsKilled);
            DataManager.instance.Save(3);
            stored = true;
            return true;
        } else if (starCount == DataManager.instance.maxStarCount3) {
            if (beesKilled > DataManager.instance.maxBeesKilled3) {
                DataManager.instance.SaveTop3Score(starCount, beesKilled, snailsKilled);
                DataManager.instance.Save(3);
                stored = true;
                return true;
            } if (beesKilled == DataManager.instance.maxBeesKilled3) {
                if (snailsKilled > DataManager.instance.maxSnailsKilled3) {
                    DataManager.instance.SaveTop3Score(starCount, beesKilled, snailsKilled);
                    DataManager.instance.Save(3);
                    stored = true;
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Looks for the closest point to spawn the player and respawns them in that point
    /// </summary>
    public void RespawnPlayerInPoint() {
        Vector3 closestPoint = new Vector3(Int32.MaxValue, Int32.MaxValue, Int32.MaxValue);

        // look for the closest point to respawn the player and if that point is at a distance greater than the defined threshold to respawn the player
        foreach (Transform point in availablePointsToSpawn) {
            if (Mathf.Abs(PlayerController.instance.transform.position.x - point.transform.position.x) 
                < Mathf.Abs(PlayerController.instance.transform.position.x - closestPoint.x) &&
                (PlayerController.instance.transform.position.x - leftScreenLimit.transform.position.x > distanceFromLeftSide))
                closestPoint = point.transform.position;
        }

        // subtract the required amount of stars to continue
        starCount -= starsToContinue;
        UIController.instance.UpdateStarCountText(starCount);

        // move the player to the closest point and resume the game
        PlayerController.instance.transform.position = closestPoint;
        PlayerFeedback.instance.ShowRespawnEffect(closestPoint, transform.rotation);
        PlayerController.instance.UnFreezeMovement();
        PlayerDeathController.instance.dead = false;
        PlayerDeathController.instance.collider.enabled = true;

        UIController.instance.EnableContinuePanel(false);

        GhostController.instance.ResumeMovement();
        PlayerController.instance.ActivateInvencibility(invencibilityAfterRespawn);
        alreadyContinued = true;
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public void Restart() {
        Time.timeScale = 1;
        //MusicManager.instance.PitchRegular();
        // get the index of the current scene and re-load it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


