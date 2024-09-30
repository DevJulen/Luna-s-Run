using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public Transform upperPoint;
    public Vector3 originalPoint;
    public bool moveUp = true;
    public float moveTime = 1.5f;
    public float restTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originalPoint = transform.position;
        StartCoroutine(BeeMovementCoroutine());
    }

    /// <summary>
    /// Moves the bee up to the upper point and back to the starting point in loop
    /// </summary>
    /// <returns></returns> 
    private IEnumerator BeeMovementCoroutine() {
        while (true) {
            Vector3 startingPos = transform.position;
            float counter = 0;
            while (counter < moveTime) {
                transform.position = Vector3.Lerp(startingPos, moveUp ? upperPoint.transform.position : originalPoint, counter / moveTime);
                counter += Time.deltaTime;
                yield return null;
            }
            moveUp = !moveUp;
            yield return new WaitForSeconds(restTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER)) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            
            if (player.invencibilityCounter > 0) {
                transform.gameObject.SetActive(false);
                PlayerFeedback.instance.ShowDeathEffect(transform.position, transform.rotation);
            } else player.ActivateBeeEffect();
        }
    }
}
