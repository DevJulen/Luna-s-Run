using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class SectionManager : MonoBehaviour
{
    public Section[] sectionPrefabs;
    public Transform sectionContainer;

    public Section currentSection;

    public int initialPrewarm = 4;

    // to keep track of the four lastly generated sections
    public Queue<int> lastSpawnedSections = new Queue<int>();

    public static SectionManager instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        // if the section container has not been initialized, we use the section manager as the container
        if (sectionContainer == null) sectionContainer = transform;
        for (int i = 0; i < initialPrewarm; i++) {
            SpawnSection();
        }
    }

    /// <summary>
    /// Instances and positions a new section
    /// </summary>
    [ContextMenu("SpawnSectionTest")] // doesn't happen in-game, only for testing if we don't explicitly call the method
    public void SpawnSection() {
        // getting a new valid section randomly 
        Section newSection = ChooseValidSection();

        if (newSection == null) {
            Debug.LogWarning("A null section can't be spawned.");
            return;
        }

        // vector that will store the offset to place the new section
        Vector3 nextPositionOffset = Vector3.zero;
        // calculate the offset by adding the two half points of the current section and the section to be generated
        nextPositionOffset.x = currentSection.halfWidth + newSection.halfWidth;
        // instantiate a new game object from a prefab and store as a reference the current section
        currentSection = Instantiate(newSection, currentSection.transform.position + nextPositionOffset, Quaternion.identity, sectionContainer);
    }

    /// <summary>
    /// Checks if that section is in the four lastly spawned sections and if it's not returns that section to be spawned
    /// </summary>
    /// <returns></returns>
    public Section ChooseValidSection() {
        bool foundValidSection = false;
        
        while(!foundValidSection) {
            // choose a section randomly
            Section section = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];
            // if the section is not within the four lastly spawned sections
            if(!lastSpawnedSections.Contains(section.id)) {
                // dequeue only if the section to be spawned is not one on the initial prewarm
                if(lastSpawnedSections.Count >= initialPrewarm) lastSpawnedSections.Dequeue();
                // mark that section as spawned
                lastSpawnedSections.Enqueue(section.id);
                return section;
            }
        }
        return null;
    }
}
