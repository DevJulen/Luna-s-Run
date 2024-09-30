using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    // the target that the camera will follow, the old man in this case
    public Transform target;
    [Range(-2, 2)] public float offsetX = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        // If the tracking in any axis is activated, update the camera's position + the offset
        newPos.x = target.position.x + offsetX;
        transform.position = newPos;
    }
}
