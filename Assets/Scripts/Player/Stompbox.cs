using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stompbox : MonoBehaviour
{
    public bool snailDead = false;

    public static Stompbox instance;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag(Constants.TAG_ENEMY_STOMP) || collision.CompareTag(Constants.TAG_SNAIL_STOMP)) {
            PlayerController player = transform.parent.gameObject.GetComponent<PlayerController>();
            // if the component exists, activate the effect
            if (player != null) {
                player.BounceOnEnemy();
                collision.gameObject.transform.parent.gameObject.SetActive(false);
                if (collision.CompareTag(Constants.TAG_SNAIL_STOMP)) {
                    snailDead = true;
                    // if we stomp in a snail stomp box, instantiate the death effect in its parent's position, the snail in this case
                    PlayerFeedback.instance.ShowDeathEffect(collision.gameObject.transform.parent.transform.position, player.transform.rotation);
                    GameManager.instance.AddKilledEnemy(true);
                    snailDead = false;
                } else {
                    GameManager.instance.AddKilledEnemy(collision.gameObject.transform.parent.CompareTag(Constants.TAG_SNAIL));
                    PlayerFeedback.instance.ShowDeathEffect(collision.gameObject.transform.position, player.transform.rotation);
                }
            }
        }
    }
}
