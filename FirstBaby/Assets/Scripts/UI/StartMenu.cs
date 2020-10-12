using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string GameScene= string.Empty;
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Single);// Loads the Dungeon Scene
    }
    public void QuitGame()
    {
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();// Closes the application
        #endif
    }
}
