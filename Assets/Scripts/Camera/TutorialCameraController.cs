using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCameraController : MonoBehaviour
{
    [Header("SectionMovement")]
    public float xPosOffset = 3.7f;
    public float cameraMoveTime = 0.2f;

    [Header("CameraShake")]
    public float duration = 1.5f;
    public float magnitude = 0.1f;
    public float moveOffset = 0.2f;

    public static TutorialCameraController instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    /// <summary>
    /// Starts the coroutine to move the camera from one section to the next one
    /// </summary>
    public void MoveCamera()
    {
        StartCoroutine(MoveCameraCoroutine());
    }

    /// <summary>
    /// Moves the camera from one section to another smoothly
    /// </summary>
    /// <returns></returns> 
    private IEnumerator MoveCameraCoroutine()
    {
        float counter = 0;
        Vector3 target = new Vector3(transform.position.x + xPosOffset, transform.position.y, transform.position.z);
        while (counter < cameraMoveTime)
        {
            transform.position = Vector3.Lerp(transform.position, target, counter / cameraMoveTime);
            counter += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Checks if the active scene is the tutorial scene, if so, starts the camera shaking coroutine
    /// </summary>
    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCoroutine());
    }

    /// <summary>
    /// Coroutine for shaking the camera
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShakeCameraCoroutine()
    {
        Vector3 originalPos = transform.position;

        float counter = duration;

        while(counter > 0f)
        {
            float xOffset = Random.Range(-moveOffset, moveOffset) * magnitude;
            float yOffset = Random.Range(-moveOffset, moveOffset) * magnitude;

            transform.position = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, transform.position.z);

            counter -= Time.deltaTime;

            yield return null;
        }
        transform.position = originalPos;
    }
}
