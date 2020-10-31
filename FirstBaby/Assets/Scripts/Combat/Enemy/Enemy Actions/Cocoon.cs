using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocoon : EnemyAction
{
    public int newBaseValue=3, TurnCount=2, newTurnCount=0;
    public float newDefenseMultiplier, newShieldMultiplier;
    public bool CustomDefense, CustomDuration, CustomShieldMultiplier;
    protected override void Start()
    {
        base.Start();
        if(Customizable)
        {
            if (CustomDefense)
                newBaseValue =(int)(newBaseValue * newDefenseMultiplier);
            if (CustomDuration)
                TurnCount = newTurnCount;// Custom Duration
            if (CustomShieldMultiplier)
                myInfo.BaseShield = (int)(myInfo.BaseShield * newShieldMultiplier);// New shield value
        }
    }
    public override void Effect()
    {
        Debug.Log("Cocooned");
        DefenseUpEffect DefenseUptoAdd = myClass.gameObject.AddComponent<DefenseUpEffect>() as DefenseUpEffect;// Add a lasting effect of Curl Up that adds
        DefenseUptoAdd.InitializeEffect(newBaseValue, 0, 0, 1, 1, TurnCount);
        IncapacitatedEffect IncapacitatedtoAdd = myClass.gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Add a lasting effect of Curl Up that adds
        IncapacitatedtoAdd.InitializeEffect(0, 0, 0, 1, 1, TurnCount);
        myClass.GainShield(CalculateAction(myInfo.BaseShield));// Gain shield
    }
}
