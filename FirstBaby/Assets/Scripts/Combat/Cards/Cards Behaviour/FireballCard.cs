using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCard : TargetCard
{ // This effect deals damage to a single enemy
    [SerializeField] private FireballOverflow myOverflow;// Reference to the fireball overflow will be used to determine how much of the fireball base damage will be dealt
    protected override void Awake()
    {
        myOverflow = GetComponent<FireballOverflow>();// Reference is defined
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
                BaseDamage = 18;// Deal damage
                myOverflow.DamagePercentage = .33f;// Deal % of base damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 28;// Deal damage
                myOverflow.DamagePercentage = .33f;// Deal % of base damage
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 36;// Deal damage
                myOverflow.DamagePercentage = .5f;// Deal % of base damage
                break;
        }
    }
}
