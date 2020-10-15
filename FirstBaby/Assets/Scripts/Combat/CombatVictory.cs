using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatVictory : MonoBehaviour
{
    // Start is called before the first frame update
    private Button EndTurnButton=null;
    private CombatPlayer Player = null;
    [SerializeField]private GameObject CardSelectionUI=null;
    [SerializeField] private RewardManager rewardManager;
    void Start()
    {
        EndTurnButton = GameObject.Find("Canvas").transform.Find("End Turn").GetComponent<Button>();// Reference to the end button is set
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is set
        rewardManager = GameObject.Find("Reward Manager").GetComponent<RewardManager>();// Reference to the reward manager
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Victory()
    {
        EndTurnButton.gameObject.SetActive(false);// Deactivates the button
        Player.gameObject.GetComponent<Hand>().DiscardHand();// Discard the cards in hand
        //TODO: Update Card Selection before activating it
        rewardManager.FillCardSelection();// Update card options to acquire new one
        CardSelectionUI.SetActive(true);// Active the card selection UI
    }
}
