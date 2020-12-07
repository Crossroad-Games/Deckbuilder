using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatVictory : MonoBehaviour
{
    #region References
    private Button EndTurnButton=null;
    private CombatPlayer Player = null;
    [SerializeField]private GameObject CardSelectionUI=null;
    private RewardManager rewardManager;
    private CombatManager combatManager = null;
    #endregion

    #region Events
    public static Action playerVictoryEvent;
    #endregion

    void Start()
    {
        EndTurnButton = GameObject.Find("Combat Canvas").transform.Find("End Turn").GetComponent<Button>();// Reference to the end button is set
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is set
        rewardManager = GameObject.Find("Reward Manager").GetComponent<RewardManager>();// Reference to the reward manager
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
    }

    public void Victory()
    {
        playerVictoryEvent?.Invoke();// Calls the event for when player wins a combat
        EndTurnButton.gameObject.SetActive(false);// Deactivates the button
        Player.gameObject.GetComponent<Hand>().DiscardHand();// Discard the cards in hand
        //Update Card Selection before activating the CardSelection UI
        rewardManager.FillCombatCardSelection();// Update card options to acquire new one
        CardSelectionUI.SetActive(true);// Active the card selection UI
        combatManager.Won = true;
    }
}
