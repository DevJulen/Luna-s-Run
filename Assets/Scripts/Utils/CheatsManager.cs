using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsManager : MonoBehaviour
{
    public GameObject chestPrefab;

    public void Give25Stars() {
        GameManager.instance.starCount += 25;
        UIController.instance.UpdateStarCountText(GameManager.instance.starCount);
    }

    public void GiveDoubleDash() {
        UIController.instance.ChangeScoreSprite();
        PlayerController.instance.AllowDoubleDash();
    }

    public void SpawnChest() {
        Instantiate(chestPrefab, 
                    new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y-0.04f, PlayerController.instance.transform.position.z),
                    PlayerController.instance.transform.rotation);
    }
}
