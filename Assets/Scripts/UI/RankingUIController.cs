using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RankingUIController : MonoBehaviour {
    [Header("Top1Score")]
    public TMP_Text starCountText1;
    public TMP_Text beesKilledText1;
    public TMP_Text snailsKilledText1;
    public TMP_Text dateText1;
    public GameObject top1Container;

    [Header("Top1Score")]
    public TMP_Text starCountText2;
    public TMP_Text beesKilledText2;
    public TMP_Text snailsKilledText2;
    public TMP_Text dateText2;
    public GameObject top2Container;

    [Header("Top1Score")]
    public TMP_Text starCountText3;
    public TMP_Text beesKilledText3;
    public TMP_Text snailsKilledText3;
    public TMP_Text dateText3;
    public GameObject top3Container;

    [Header("Confirm")]
    public CanvasGroup confirmPanel;

    public static RankingUIController instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        // update the information in each row
        starCountText1.text = DataManager.instance.maxStarCount1.ToString();
        beesKilledText1.text = DataManager.instance.maxBeesKilled1.ToString();
        snailsKilledText1.text = DataManager.instance.maxSnailsKilled1.ToString();
        dateText1.text = DataManager.instance.maxScoreDate1;

        starCountText2.text = DataManager.instance.maxStarCount2.ToString();
        beesKilledText2.text = DataManager.instance.maxBeesKilled2.ToString();
        snailsKilledText2.text = DataManager.instance.maxSnailsKilled2.ToString();
        dateText2.text = DataManager.instance.maxScoreDate2;

        starCountText3.text = DataManager.instance.maxStarCount3.ToString();
        beesKilledText3.text = DataManager.instance.maxBeesKilled3.ToString();
        snailsKilledText3.text = DataManager.instance.maxSnailsKilled3.ToString();
        dateText3.text = DataManager.instance.maxScoreDate3;

        ClearDataFromPanel();
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// If no record has been made, clears the panel from the information displayed
    /// </summary>
    public void ClearDataFromPanel() {
        // if the star count in each top is 0 means that the corresponding top hasn't been achieved yet
        if (DataManager.instance.maxStarCount1 == 0) {
            top1Container.SetActive(false);
        }
        if (DataManager.instance.maxStarCount2 == 0) {
            top2Container.SetActive(false);
        }
        if (DataManager.instance.maxStarCount3 == 0) {
            top3Container.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the  data has been removed and if not, shows the confirmation panel
    /// </summary>
    /// <param name="enable"></param>
    public void ShowConfirmationPanel(bool enable) {
        // if the data is already deleted return so the confirmation panel doesn't pop up
        if (DataManager.instance.maxStarCount1 == 0) return;

        EnableConfirmationPanel(enable);
    }

    /// <summary>
    /// Activates or deactivates the confirmation panel based on the parameter
    /// </summary>
    /// <param name="enable"></param>
    public void EnableConfirmationPanel(bool enable) {

        // making the canvas visible or invisible
        confirmPanel.alpha = enable ? 1 : 0;
        // making the canvas interactable or not
        confirmPanel.interactable = enable;
        // making the canvas block raycasts or not
        confirmPanel.blocksRaycasts = enable;
    }
}
