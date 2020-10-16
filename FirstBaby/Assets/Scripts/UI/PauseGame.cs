using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool IsPaused { get; private set; }
    public static Action<bool> PauseEvent;// Event that is called whenever the game is paused
    [SerializeField] private GameObject Menu=null;// Reference to the Menu script is set on the inspector
    void Start()
    {
        IsPaused = false;// Starts as a false
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))// If the user hits ESC
        {
            Pause();
            Menu.SetActive(true);
        }
    }
    public void Pause()
    {
        IsPaused = true;
        PauseEvent?.Invoke(true);
    }
    public void UnPause()
    {
        Menu.SetActive(false);
        IsPaused = false;
        PauseEvent?.Invoke(false);
    }
}
