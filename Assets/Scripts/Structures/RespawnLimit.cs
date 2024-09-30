using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnLimit : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;
    public Transform playerSpawnPoint;

    [Header("Pickups")]
    public GameObject carrot;
    public Transform carrotSpawnPoint;
    public GameObject carrotRespawnEffect;
    public bool hasCarrot = false;

    public GameObject[] starPrefabs;
    public Transform[] starSpawnPoints;
    public GameObject starRespawnEffect;

    public float respawnAnimationTime = 0.5f;

    public GameObject[] pickUpsInLevel;
    public int pickedStars = 0;

    public static RespawnLimit instance;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag(Constants.TAG_PLAYER)) {
            RespawnPlayer();
            DestroyAllPickUps();
            RespawnStars();
            RespawnCarrot();
            GameManager.instance.RemoveStarCount(GameManager.instance.pickedStarsInTutorialSection);
            GameManager.instance.pickedStarsInTutorialSection = 0;
        }
    }

    /// <summary>
    /// Respawns the player back in the spawn point of that section
    /// </summary>
    public void RespawnPlayer() {
        player.rigidBody.velocity = Vector2.zero;
        player.transform.position = playerSpawnPoint.position;
        PlayerController.instance.canDoubleDash = false;
        PlayerFeedback.instance.ShowRespawnEffect(PlayerController.instance.respawnSpawnPoint.position, PlayerController.instance.respawnSpawnPoint.rotation);
    }

    /// <summary>
    /// Respawns the stars in the current level
    /// </summary>
    public void RespawnStars() {
        if (starPrefabs.Length == 0) return;

        for(int i = 0; i < starPrefabs.Length; i++) {
            GameObject clone = Instantiate(starPrefabs[i], starSpawnPoints[i].position, starPrefabs[i].transform.rotation);
            StartCoroutine(RespawnEffect(starSpawnPoints[i], true));
            pickUpsInLevel[i] = clone;
        }
    }

    /// <summary>
    /// Respawns a carrot if the section has one
    /// </summary>
    public void RespawnCarrot() {
        if (hasCarrot) {
            GameObject clone = Instantiate(carrot, carrotSpawnPoint.position, carrot.transform.rotation);
            StartCoroutine(RespawnEffect(carrotSpawnPoint, false));
            pickUpsInLevel[pickUpsInLevel.Length - 1] = clone;
        }
    }

    /// <summary>
    /// Destroys all pickups in the level
    /// </summary>
    public void DestroyAllPickUps() {
        if (pickUpsInLevel.Length == 0) return;
        foreach(GameObject pickUp in pickUpsInLevel) {
            Destroy(pickUp);
        }
    }

    /// <summary>
    /// Coroutine to spawn the respawning effect for the pickups
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="star"></param>
    /// <returns></returns>
    public IEnumerator RespawnEffect(Transform spawnPoint, bool star) {
        GameObject clone = Instantiate(star ? starRespawnEffect : carrotRespawnEffect, spawnPoint.position, spawnPoint.transform.rotation);
        yield return new WaitForSeconds(respawnAnimationTime);
        Destroy(clone);
    }
}
