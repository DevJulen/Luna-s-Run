using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(chestPrefab, transform.position, transform.rotation, transform);
    }
}
