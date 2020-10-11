using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private PauseGame PauseMaster;
    private string dataPath;// Where it will be saved to in PC
    void Start()
    {
        PauseMaster = GameObject.Find("Pause Master").GetComponent<PauseGame>();// Acquires the reference
        dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName");// Saves the information at this location
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void SaveGame()// This will save all the information on this script to the file
    {
        string jsonString = JsonUtility.ToJson(this);// Transforms the Data to Json format
        Debug.Log(Application.persistentDataPath);
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
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
