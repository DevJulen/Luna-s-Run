using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER)) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null) return;

            if (player.invencibilityCounter > 0) {
                transform.gameObject.SetActive(false);
                PlayerFeedback.instance.ShowDeathEffect(transform.position, transform.rotation);
            } else  player.ActivateSnailEffect();
        }
    }
}
