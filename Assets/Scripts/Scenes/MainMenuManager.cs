using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        animator.SetBool("LoadScreen", false);
        MusicManager.instance.PlayMainMenu();
    }

    /// <summary>
    /// Closes the game (Only works after building)
    /// </summary>
    public void QuitGame() {

#if UNITY_STANDALONE // una vez buildeado, afecta a android, ios, pc etc. Cualquier versión que esté ejecutandose fuera de Unity
        // closes the game in the build
        Application.Quit();
#endif

#if UNITY_EDITOR
        // deactivates the game execution in Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
