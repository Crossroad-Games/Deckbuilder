using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialLayerSetup : MonoBehaviour
{
    [SerializeField] private List<GameObject> Rooms= new List<GameObject>();
    private void Awake()
    {
        SceneManager.sceneLoaded += Init;
    }
    public void Init(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject Room in Rooms)// Go through each Room set on the inspector
            if (Room != null)// If Room not null
                this.GetComponent<ChangeRoom>().SetLayerRecursively(Room, 29,false);// Ghost it
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
    }
}