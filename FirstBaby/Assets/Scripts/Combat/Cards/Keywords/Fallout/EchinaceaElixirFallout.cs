using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchinaceaElixirFallout : FalloutKeyword
{
    [SerializeField] private int AmountofStacks = 5;

    public override void FalloutEffect()
    {
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericDefenseKeywordEffect/GenericDefenseKeywordEffect"), new Vector3(-1.92f, 0.25f, -0.23f), Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericDefenseKeywordEffect>().virtualCard = this.myCard;

        //Instantiate card UI
        GameObject canvas = GameObject.Find("Canvas");
        GameObject cardUI = Instantiate(Resources.Load("UI/Cards UI/" + myCard.cardInfo.ID), keywordCardUIPosition, Quaternion.identity, canvas.transform) as GameObject;
        visualEffect.GetComponent<GenericDefenseKeywordEffect>().cardUI = cardUI;
    }

    public override void DealEffect()
    {
        combatPlayer.GainShield(8);
        WallEfect preExistantEffect = combatPlayer.GetComponent<WallEfect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            WallEfect effectToAdd = combatPlayer.gameObject.AddComponent<WallEfect>() as WallEfect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
    }
}
