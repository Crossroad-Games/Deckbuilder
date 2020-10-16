using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string GameScene= string.Empty;
    public void StartGame()
    {
        if (CombatGameData.Current != null)// If there is a combat save
        {
            if (CombatGameData.Current.CombatScene != string.Empty)// If there is saved scene string
                GameScene = CombatGameData.Current.CombatScene;// Go to this scene
        }
        else
        {
            if (DungeonGameData.Current != null)// If there is a dungeon save
                if (DungeonGameData.Current.DungeonScene != string.Empty)// If there is a saved scene string
                    GameScene = DungeonGameData.Current.DungeonScene;// Go to this scene
        }
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
