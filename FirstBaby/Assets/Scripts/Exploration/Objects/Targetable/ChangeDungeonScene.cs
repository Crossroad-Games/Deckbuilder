using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDungeonScene : Targetable
{
    [SerializeField] private string DungeonSceneName;
    public override void ExecuteAction()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DungeonSceneName, LoadSceneMode.Single);// Loads the combat scene
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
