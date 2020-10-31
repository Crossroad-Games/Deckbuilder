using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisruptiveBlow : EnemyAction
{
    public float ShieldDepletion=.25f;
    public int newTurnCount;
    public bool CustomShieldDepletion = false, CustomDuration=false;
    public float newDepletion;
    private int TurnDuration=2;
    private void Awake()
    {
        if (Customizable)
        {
            if (CustomShieldDepletion)
                ShieldDepletion *= newDepletion;// Shield Depletion updated if Customized
            if (CustomDuration)
                TurnDuration = newTurnCount;// New duration if customized
        }
    }
    public override void Effect()
    {
        Debug.Log("Used this skill");
        Debug.Log(TurnDuration);
        Player.LoseShield(CalculateAction(Mathf.FloorToInt(Player.myData.PlayerShield * ShieldDepletion)));// Deplete the player's shield by a preset amount
        DisruptedEffect DisruptToAdd = Player.gameObject.AddComponent<DisruptedEffect>() as DisruptedEffect;// Apply Disrupted status to the player, messing with their CD pile
        DisruptToAdd.InitializeEffect(0, 0, 0, 1, 1,TurnDuration);// Duration of the effect
    }
    public override void ShowValue()
    {
        ActionValueText.text = $"{ CalculateAction(Mathf.FloorToInt(Player.myData.PlayerShield * ShieldDepletion))}";// Show how much shield the player will lose
    }
}
