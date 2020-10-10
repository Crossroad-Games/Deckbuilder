using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private PauseGame PauseMaster;
    void Start()
    {
        PauseMaster = GameObject.Find("Pause Master").GetComponent<PauseGame>();// Acquires the reference
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ContinuePlay() => PauseMaster.UnPause();
    public void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();// Closes the application
        #endif
    }
}
