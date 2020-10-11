using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool IsPaused { get; private set; }
    [SerializeField] private GameObject Menu;// Reference to the Menu script is set on the inspector
    [SerializeField] private CombatPlayer Player;
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
        Player.FlipEndButton(false);// The player can't end their turn
    }
    public void UnPause()
    {
        Menu.SetActive(false);
        IsPaused = false;
        Player.FlipEndButton(true);// The player can end their turn
    }
}
