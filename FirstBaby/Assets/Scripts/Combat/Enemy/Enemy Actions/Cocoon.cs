using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class Cocoon : EnemyAction
{
    public int AmountofStacks=3, newStacksCount=0, TurnCount;
    public float newShieldMultiplier;
    public bool CustomStacks, CustomShieldMultiplier;
    public override IEnumerator Effect()
    {
        Debug.Log("Cocooned");
        DefenseUpEffect preExistantEffect = myClass.GetComponent<DefenseUpEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            DefenseUpEffect effectToAdd = myClass.gameObject.AddComponent<DefenseUpEffect>() as DefenseUpEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
        IncapacitatedEffect IncapacitatedtoAdd = myClass.gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Add a lasting effect of Curl Up that adds
        IncapacitatedtoAdd.InitializeEffect(0, 0, 0, 1, 1, AmountofStacks);
        myClass.GainShield(CalculateAction(myInfo.BaseShield));// Gain shield
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }

    protected override void Start()
    {
        base.Start();
        if(Customizable)
        {
            if (CustomStacks)
                AmountofStacks = newStacksCount;// Custom Duration
            if (CustomShieldMultiplier)
                myInfo.BaseShield = (int)(myInfo.BaseShield * newShieldMultiplier);// New shield value
        }
        TurnCount = AmountofStacks;
    }
}
