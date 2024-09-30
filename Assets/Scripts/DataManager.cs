using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("No1Score")]
    public int maxStarCount1;
    public int maxSnailsKilled1;
    public int maxBeesKilled1;
    public string maxScoreDate1;

    [Header("No2Score")]
    public int maxStarCount2;
    public int maxSnailsKilled2;
    public int maxBeesKilled2;
    public string maxScoreDate2;

    [Header("No3Score")]
    public int maxStarCount3;
    public int maxSnailsKilled3;
    public int maxBeesKilled3;
    public string maxScoreDate3;

    public static DataManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        Load();
    }

    /// <summary>
    /// Saves the information depending on what top score has the player achieved
    /// </summary>
    /// <param name="top"></param>
    public void Save(int top) {
        // save the top 1 score
        if (top == 1) {
            PlayerPrefs.SetInt(Constants.MAX_STAR_COUNT1, maxStarCount1);
            PlayerPrefs.SetInt(Constants.MAX_SNAILS_KILLED1, maxSnailsKilled1);
            PlayerPrefs.SetInt(Constants.MAX_BEES_KILLED1, maxBeesKilled1);
            PlayerPrefs.SetString(Constants.MAX_SCORE_DATE1, maxScoreDate1);
        } else if (top == 2) {
            // save the top 2 score
            PlayerPrefs.SetInt(Constants.MAX_STAR_COUNT2, maxStarCount2);
            PlayerPrefs.SetInt(Constants.MAX_SNAILS_KILLED2, maxSnailsKilled2);
            PlayerPrefs.SetInt(Constants.MAX_BEES_KILLED2, maxBeesKilled2);
            PlayerPrefs.SetString(Constants.MAX_SCORE_DATE2, maxScoreDate2);
        } else if (top == 3) { 
            // save the top 3 score
            PlayerPrefs.SetInt(Constants.MAX_STAR_COUNT3, maxStarCount3);
            PlayerPrefs.SetInt(Constants.MAX_SNAILS_KILLED3, maxSnailsKilled3);
            PlayerPrefs.SetInt(Constants.MAX_BEES_KILLED3, maxBeesKilled3);
            PlayerPrefs.SetString(Constants.MAX_SCORE_DATE3, maxScoreDate3);
        }
    }

    /// <summary>
    /// Loads the stored information
    /// </summary>
    public void Load() {
        // load the top 1 score
        if (PlayerPrefs.HasKey(Constants.MAX_STAR_COUNT1)) maxStarCount1 = PlayerPrefs.GetInt(Constants.MAX_STAR_COUNT1);
        if (PlayerPrefs.HasKey(Constants.MAX_SNAILS_KILLED1)) maxSnailsKilled1 = PlayerPrefs.GetInt(Constants.MAX_SNAILS_KILLED1);
        if (PlayerPrefs.HasKey(Constants.MAX_BEES_KILLED1)) maxBeesKilled1 = PlayerPrefs.GetInt(Constants.MAX_BEES_KILLED1);
        if (PlayerPrefs.HasKey(Constants.MAX_SCORE_DATE1)) maxScoreDate1 = PlayerPrefs.GetString(Constants.MAX_SCORE_DATE1);

        // load the top 2 score
        if (PlayerPrefs.HasKey(Constants.MAX_STAR_COUNT2)) maxStarCount2 = PlayerPrefs.GetInt(Constants.MAX_STAR_COUNT2);
        if (PlayerPrefs.HasKey(Constants.MAX_SNAILS_KILLED2)) maxSnailsKilled2 = PlayerPrefs.GetInt(Constants.MAX_SNAILS_KILLED2);
        if (PlayerPrefs.HasKey(Constants.MAX_BEES_KILLED2)) maxBeesKilled2 = PlayerPrefs.GetInt(Constants.MAX_BEES_KILLED2);
        if (PlayerPrefs.HasKey(Constants.MAX_SCORE_DATE2)) maxScoreDate2 = PlayerPrefs.GetString(Constants.MAX_SCORE_DATE2);

        // load the top 3 score
        if (PlayerPrefs.HasKey(Constants.MAX_STAR_COUNT3)) maxStarCount3 = PlayerPrefs.GetInt(Constants.MAX_STAR_COUNT3);
        if (PlayerPrefs.HasKey(Constants.MAX_SNAILS_KILLED3)) maxSnailsKilled3 = PlayerPrefs.GetInt(Constants.MAX_SNAILS_KILLED3);
        if (PlayerPrefs.HasKey(Constants.MAX_BEES_KILLED3)) maxBeesKilled3 = PlayerPrefs.GetInt(Constants.MAX_BEES_KILLED3);
        if (PlayerPrefs.HasKey(Constants.MAX_SCORE_DATE3)) maxScoreDate3 = PlayerPrefs.GetString(Constants.MAX_SCORE_DATE3);
    }

    /// <summary>
    /// Updates the score for the top 1 with the given information
    /// </summary>
    /// <param name="starCount"></param>
    /// <param name="beesKilled"></param>
    /// <param name="snailsKilled"></param>
    public void SaveTop1Score(int starCount, int beesKilled, int snailsKilled) {
        maxStarCount1 = starCount;
        maxBeesKilled1 = beesKilled;
        maxSnailsKilled1 = snailsKilled;
        maxScoreDate1 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();
    }

    /// <summary>
    /// Updates the score for the top 2 with the given information
    /// </summary>
    /// <param name="starCount"></param>
    /// <param name="beesKilled"></param>
    /// <param name="snailsKilled"></param>
    public void SaveTop2Score(int starCount, int beesKilled, int snailsKilled) {
        maxStarCount2 = starCount;
        maxBeesKilled2 = beesKilled;
        maxSnailsKilled2 = snailsKilled;
        maxScoreDate2 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();
    }

    /// <summary>
    /// Updates the score for the top 3 with the given information
    /// </summary>
    /// <param name="starCount"></param>
    /// <param name="beesKilled"></param>
    /// <param name="snailsKilled"></param>
    public void SaveTop3Score(int starCount, int beesKilled, int snailsKilled) {
        maxStarCount3 = starCount;
        maxBeesKilled3 = beesKilled;
        maxSnailsKilled3 = snailsKilled;
        maxScoreDate3 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();
    }

    /// <summary>
    /// Deletes all the stored data
    /// </summary>
    public void ClearData() {

        PlayerPrefs.DeleteAll();

        maxStarCount1 = 0;
        maxBeesKilled1 = 0;
        maxSnailsKilled1 = 0;
        maxScoreDate1 = "";
        Save(1);

        maxStarCount2 = 0;
        maxBeesKilled2 = 0;
        maxSnailsKilled2 = 0;
        maxScoreDate2 = "";
        Save(2);

        maxStarCount3 = 0;
        maxBeesKilled3 = 0;
        maxSnailsKilled3 = 0;
        maxScoreDate3 = "";
        Save(3);

        RankingUIController.instance.ClearDataFromPanel();
        RankingUIController.instance.EnableConfirmationPanel(false);
    }

    /// <summary>
    /// Deletes all the stored data
    /// </summary>
    [ContextMenu("ClearDataContextMenu")]
    public void ClearDataContextMenu() {
        PlayerPrefs.DeleteAll();
        maxStarCount1 = 0;
        maxBeesKilled1 = 0;
        maxSnailsKilled1 = 0;
        maxScoreDate1 = "";
        Save(1);

        maxStarCount2 = 0;
        maxBeesKilled2 = 0;
        maxSnailsKilled2 = 0;
        maxScoreDate2 = "";
        Save(2);

        maxStarCount3 = 0;
        maxBeesKilled3 = 0;
        maxSnailsKilled3 = 0;
        maxScoreDate3 = "";
        Save(3);
    }

    /// <summary>
    /// Fills the data with placeholder values
    /// </summary>
    [ContextMenu("FillData")]
    public void FillData() {
        maxStarCount1 = 500;
        maxBeesKilled1 = 30;
        maxSnailsKilled1 = 20;
        maxScoreDate1 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();

        maxStarCount2 = 400;
        maxBeesKilled2 = 30;
        maxSnailsKilled2 = 20;
        maxScoreDate2 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();

        maxStarCount3 = 400;
        maxBeesKilled3 = 30;
        maxSnailsKilled3 = 10;
        maxScoreDate3 = DateTime.Now.ToString(Constants.DATE_FORMAT, CultureInfo.InvariantCulture).ToUpper();

        Save(1);
        Save(2);
        Save(3);
    }
}
