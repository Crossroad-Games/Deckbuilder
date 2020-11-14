﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowder : TargetCard
{
    [SerializeField] private BuckthornPowderFallout myFallout;// Will be used to manipulate values on the level rank method
    protected override void Awake()
    {
        myFallout = GetComponent<BuckthornPowderFallout>();// Reference is defined
        base.Awake();
    }
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        return base.CardEffect();
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                myFallout.AmountofStacks = 4;// How many agony stacks this card will use
                myFallout.DamageMultiplier = 2f;// Damage multiplier when agony threshold is met
                BaseDamage = 10;// Base Damage
                break;
            case 1:// One LVL higher than base
                myFallout.AmountofStacks = 6;// How many agony stacks this card will use
                myFallout.DamageMultiplier = 2f;// Damage multiplier when agony threshold is met
                BaseDamage = 12;// Base Damage
                break;
            case 2:// Two LVLs higher than base
                myFallout.AmountofStacks = 6;// How many agony stacks this card will use
                myFallout.DamageMultiplier = 3f;// Damage multiplier when agony threshold is met
                BaseDamage = 15;// Base Damage
                break;
        }
    }
}
