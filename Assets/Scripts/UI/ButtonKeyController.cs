using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonKeyController : MonoBehaviour
{
    [Header("InteractKeyButton")]
    public GameObject keyboardKey;
    public GameObject xboxButton;

    // Start is called before the first frame update
    void Start()
    {
        ChangeButtonKeyControl();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeButtonKeyControl();
    }

    /// <summary>
    /// Checks if the last used is a key or a button and changes the object showing the control
    /// </summary>
    public void ChangeButtonKeyControl()
    {
        if(GameManager.instance.lastPressed == Constants.KEYBOARD_MOUSE)
        {
            keyboardKey.SetActive(true);
            xboxButton.SetActive(false);
        }

        if (GameManager.instance.lastPressed == Constants.GAMEPAD)
        {
            keyboardKey.SetActive(false);
            xboxButton.SetActive(true);
        }
    }
}
