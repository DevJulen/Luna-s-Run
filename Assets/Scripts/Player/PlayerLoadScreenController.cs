using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLoadScreenController : MonoBehaviour
{
    public Animator animator;

    public Transform rightPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("LoadScreen", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, rightPoint.position, 0.8f * Time.deltaTime);

        if(transform.position.x >= rightPoint.position.x)
        {
            SceneManager.LoadScene(TransitionController.instance.sceneToGo);
        }
    }
}
