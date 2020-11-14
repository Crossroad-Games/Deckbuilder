using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLanceCard : TargetCard
{
    [SerializeField] private IceLanceWildcast myWildCast;// Reference will be used on the LevelRanks method to determine how long will the incapacitate effect last and how much base damage shall be dealt
    protected override void Awake()
    {
        myWildCast = GetComponent<IceLanceWildcast>();// Reference is defined
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
                myWildCast.IncapacitatedDuration = 1;// Apply X incapacitate effects
                myWildCast.DamagePercentage = 0;// Deal X% base damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 25;// Deal damage
                myWildCast.IncapacitatedDuration = 1;// Apply X incapacitate effects
                myWildCast.DamagePercentage = .2f;// Deal X% base damage
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 36;// Deal damage
                myWildCast.IncapacitatedDuration = 2;// Apply X incapacitate effects
                myWildCast.DamagePercentage = .25f;// Deal X% base damage
                break;
        }
    }
}
