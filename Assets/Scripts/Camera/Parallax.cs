using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // background speed, at 0.5 we go at the same speed as the camera
    [Range(0f, 0.5f)]
    public float speedFactor = 0.066f;
    // position to control the offset of the texture
    private Vector2 pos = Vector2.zero;
    // reference to the main camera
    private Camera cam;
    // camera's previous position
    private Vector2 camOldPos;
    // reference to the background's renderer for accessing its material
    private Renderer rend;

    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
        camOldPos = cam.transform.position;
        rend = GetComponentInChildren<Renderer>();

        Vector2 backgroundHalfSize = new Vector2((cam.orthographicSize * Screen.width) / Screen.height, cam.orthographicSize);

        // adjusting the background's scale to fit the screen size
        transform.localScale = new Vector3(backgroundHalfSize.x * 2, backgroundHalfSize.y * 2, transform.localScale.z);

        // _MainTex stored in unity explorer -> click material -> Edit
        // adjusting the tilling to be proportional to the scale of the quad
        // we leave it at half size to reduce the number of repetitions
        rend.material.SetTextureScale("_MainTex", backgroundHalfSize);
    }

    // Update is called once per frame
    void Update() {
        // how much has the camera moved. In this case, this came doesn't have vertical movement so the second coordinate could be 0
        Vector2 camVar = new Vector2(cam.transform.position.x - camOldPos.x, cam.transform.position.y - camOldPos.y);

        // modifying the offset to be applied to the texture
        // multiply by the speedFactor to modify its movement relative to the camera
        pos.Set(pos.x + (camVar.x * speedFactor), pos.y + (camVar.y * speedFactor));

        // applying the offset to the main texture
        rend.material.SetTextureOffset("_MainTex", pos);

        // updating the old position with the current position before moving on to the next frame
        camOldPos = cam.transform.position;
    }
}
