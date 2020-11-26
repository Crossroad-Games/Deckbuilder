using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockArmorCard : NonTargetCard
{
    [SerializeField] private int AmountofStacks = 3;
    // This effect creates a shield that will protect the player by this amount
    public override IEnumerator CardEffect()
    {
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericDefense/GenericDefense"), new Vector3(-1.92f, 0.25f, -0.23f), Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericDefenseEffect>().card = this;
        visualEffect.GetComponent<GenericDefenseEffect>().dealEffect = true;
        yield return StartCoroutine(base.CardEffect());
    }

    public override void DealEffect()
    {
        effectFinished = true;
        PlayerDefenseUpEffect preExistantEffect = Player.GetComponent<PlayerDefenseUpEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            PlayerDefenseUpEffect effectToAdd = Player.gameObject.AddComponent<PlayerDefenseUpEffect>() as PlayerDefenseUpEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
    }

    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseShield = 15;// Gain X ward
                AmountofStacks = 3;// Gain X stacks of Defense
                break;
            case 1:// One LVL higher than base
                BaseShield = 30;// Gain X Ward
                AmountofStacks = 4;// Gain X stacks of Defense
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                BaseShield = 45;// Gain X Ward
                AmountofStacks = 5;// Gain X stacks of Defense
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Ward {thisVirtualCard.CalculateAction(BaseShield)}\nArmor Up {AmountofStacks}";
    }
}
