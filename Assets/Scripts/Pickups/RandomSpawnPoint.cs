using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoint : MonoBehaviour
{
    [Header("GoldenCarrot")]
    public GameObject goldenCarrot;
    public bool spawnCarrot = false;
    public float goldenCarrotSpawnProbability = 0.8f;

    [Header("Stars")]
    public Pickup[] starPrefabs;
    public float purpleStarSpawnProbability = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPickUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns a pickup in the position where the point is 
    /// </summary>
    public void SpawnPickUp()
    {
        float prob = Random.Range(0f, 1f);
        if(!spawnCarrot) Instantiate(prob > purpleStarSpawnProbability ? starPrefabs[0] : starPrefabs[1], transform.position, transform.rotation, transform);
        else if(prob < goldenCarrotSpawnProbability) {
            Instantiate(goldenCarrot, transform.position, transform.rotation, transform);
        }
    }
}
