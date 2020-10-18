using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatDefeat : MonoBehaviour
{
    #region References
    private Button EndTurnButton = null;
    private GameObject DefeatScreen = null;
    private CombatPlayer Player = null;
    private CombatManager combatManager = null;
    #endregion

    #region Events
    public static Action playerDefeatEvent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        EndTurnButton = GameObject.Find("Canvas").transform.Find("End Turn").GetComponent<Button>();// Reference to the end button is set
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is set
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
        DefeatScreen = GameObject.Find("Defeat Screen");
    }


    public void Defeat()
    {
        playerDefeatEvent?.Invoke();// Calls the event for when player loses a combat
        EndTurnButton.gameObject.SetActive(false);// Deactivates the button
        Player.gameObject.GetComponent<Hand>().DiscardHand();// Discard the cards in hand
        DefeatScreen.SetActive(true);
        combatManager.Defeated = true;

    }

    public void Retry()
    {

    }
}
