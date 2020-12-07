using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private RewardManager rewardManager;
    [SerializeField] private GameObject CardSelectionUI = null;

    protected override void Start()
    {
        rewardManager = GameObject.Find("Reward Manager").GetComponent<RewardManager>();
    }

    public override void Actuated()
    {
        Debug.Log("Get some rewards");
        Player.GetComponent<PlayerMovement>().canMove = false; //Disable player movement
        rewardManager.FillDungeonCardSelection();
        //CardSelectionUI.SetActive(true);
        ExecuteTargetActions();
    }

    public override void LoadDungeonState()
    {
        
    }
}
