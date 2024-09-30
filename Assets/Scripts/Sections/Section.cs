using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour // Class for definint the sections that will be procedurally generated
{
    // identifier to use in the section spawn
    public int id;

    [Range(2, 100)] public int columns;
    [Range(2, 100)] public int rows;
    public Grid grid;

    public float halfWidth { get { return ((columns / 2) * grid.cellSize.x); } }
    public float width { get { return halfWidth * 2; } }
    private Transform cameraTransform;

    private bool isDestroyed = false;

    [Header("Respawn")]
    public Transform[] respawnPoints;

    // Start is called before the first frame update
    void Start() {
        AddPointsToList();
        // getting the reference to the camera transform of the camera with the tag "Main Camera"
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        CheckDestroy();
    }

    private void OnDrawGizmos() {
        if (!grid) grid = GetComponentInChildren<Grid>();
        if (grid == null) return;


        if (columns % 2 == 0 && rows % 2 == 0) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new Vector3(columns * grid.cellSize.x, rows * grid.cellSize.y));
    }

    /// <summary>
    /// Adds the respawn points of that section to the game manager list
    /// </summary>
    public void AddPointsToList() {
        // add the respawn points to the available points list in the game manager
        foreach (Transform point in respawnPoints) GameManager.instance.availablePointsToSpawn.Add(point);
    }

    /// <summary>
    /// Removes the respawn points of that section from the game manager list
    /// </summary>
    public void RemovePointsFromList() {
        // remove the respawn points from the available points list in the game manager
        foreach (Transform point in respawnPoints) GameManager.instance.availablePointsToSpawn.Remove(point);
    }

    /// <summary>
    /// Evaluates wheter the section needs to be destroyed or not
    /// </summary>
    public void CheckDestroy() {
        float leftSideOfScreen = cameraTransform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height);

        if (transform.position.x < (leftSideOfScreen - halfWidth) && !isDestroyed) {
            DestroySection();
        }
    }

    /// <summary>
    /// Destroys the section and calls for generating a new one
    /// </summary>
    private void DestroySection() {
        RemovePointsFromList();
        // instance the new section using the singleton of the Section Manager
        SectionManager.instance.SpawnSection();
        // destroying the current section before going into the next one
        Destroy(gameObject, 1.5f); // the last paremeter specifies that the section will be destroyed 3 seconds later, making time for the sounds to end
        // destruction has been called, the execution continues and after 1.5 seconds
        isDestroyed = true;
    }
}
