using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistFireCard : TargetCard
{
    // This effect deals damage to a single enemy
    [SerializeField] private int AmountofStacks=1;// How many agony stacks will be applied
    [SerializeField] private int StackConversionRate = 2;// How many agony stacks get converted into 1 damage
    public override IEnumerator CardEffect()
    {
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/Test2/Test2"),playerSpriteTransform.position,Quaternion.identity) as GameObject;
        visualEffect.GetComponent<VisualEffectTest2>().targetTransform = this.TargetEnemy.transform;
        visualEffect.GetComponent<VisualEffectTest2>().card = this;
        visualEffect.GetComponent<VisualEffectTest2>().dealEffect = true;
        yield return StartCoroutine(base.CardEffect());
    }

    public override void DealEffect()
    {
        Debug.Log("alchemist fire effect");
        Agony preExistentAgony = TargetEnemy.GetComponent<Agony>();
        if (preExistentAgony == null)
        {
            Agony effectToAdd = TargetEnemy.gameObject.AddComponent<Agony>() as Agony;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);
        }
        else
        {
            AddValue = preExistentAgony.turnCounter / StackConversionRate;
            Debug.Log(AddValue);
            preExistentAgony.AddStacks(AmountofStacks);
        }
        effectFinished = true;
    }



    protected override void TooltipListDefinition()
    {
        base.TooltipListDefinition();
        InstantiateTooltip("Agony");
    }
    public override void LevelRanks()
    {
        Debug.Log(CardLevel);
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                AmountofStacks = 1;// Apply 1 stack per use
                BaseDamage = 5;// Deal 5 damage
                StackConversionRate = 2;// +1 Damage per 2 Agony
                break;
            case 1:// One LVL higher than base
                AmountofStacks = 2;// Aplies 2 stacks per use
                BaseDamage = 7;// Deals 7 damage
                StackConversionRate = 2;// +1 Damage per 2 Agony
                break;
            case 2:// Two LVLs higher than base
                AmountofStacks = 2;// Applies 2 stacks per use
                BaseDamage = 7;// Deals 7 damage
                StackConversionRate = 1;// +1 damage per stack
                break;
        }
    }
}
