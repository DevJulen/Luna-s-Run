using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLimits : MonoBehaviour
{
    public Collider2D _collider;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // check if the player is on the right rise of the limit to move the camera to the right
        if(collision.CompareTag(Constants.TAG_PLAYER) && transform.position.x < collision.transform.position.x)
        {
            _collider.isTrigger = false;
            TutorialCameraController.instance.MoveCamera();
            GameManager.instance.pickedStarsInTutorialSection = 0;
        }
    }
}
