using System.Collections;
using UnityEngine;

public class SnailPlatformController : MonoBehaviour
{
    public float minRotationTime = 2;
    public float maxRotationTime = 3;
    public float timeBetweenRotations;
    public float rotationTime = 0.5f;
    public float rotation = 0f;

    public bool snailDead = false;

    public PlatformEffector2D oneWayEffector;

    [ContextMenu("FillComponents")]
    public void FillComponents() {
        oneWayEffector = GetComponentInChildren<PlatformEffector2D>();
    }

    void Start()
    {
        timeBetweenRotations = Random.Range(minRotationTime, maxRotationTime);
        StartCoroutine(RotatePlatform());
    }

    /// <summary>
    /// Rotates the platform and waits the time between rotations
    /// </summary>
    /// <returns></returns> 
    private IEnumerator RotatePlatform()
    {
        while(true)
        {
            
            rotation += 180f;
            float current = transform.eulerAngles.z;
            float counter = 0;
            while (counter < rotationTime)
            {
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Lerp(current, rotation, counter / rotationTime));
                counter += Time.deltaTime;
                yield return null;
            }
            rotation = rotation == 360f ? 0f : rotation;
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotation);
            oneWayEffector.transform.eulerAngles = new Vector3(oneWayEffector.transform.rotation.x, 180f, oneWayEffector.transform.rotation.z);
            if (Stompbox.instance.snailDead) yield break;
            yield return new WaitForSeconds(timeBetweenRotations);
        }
    }
}
