using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeCard : TargetCard
{
    private UnstableKeyword myUnstable;// Reference to the Unstable keyword will be used on the LevelRanks method to determine how strong the unstable effect will be
    protected override void Awake()
    {
        myUnstable = GetComponent<UnstableKeyword>();// Reference is defined
        base.Awake();
    }
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseDamage = 15;// Deal damage
                myUnstable.UnstableIntensity = 2;// Unstable X
                break;
            case 1:// One LVL higher than base
                BaseDamage = 25;// Deal damage
                myUnstable.UnstableIntensity = 3;// Unstable X
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 40;// Deal damage
                myUnstable.UnstableIntensity = 4;// Unstable X
                break;
        }
    }
}
