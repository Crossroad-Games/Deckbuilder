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
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/Test2/Test2"), playerSpriteTransform.position, Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericAttackEffect>().targetTransform = this.TargetEnemy.transform;
        visualEffect.GetComponent<GenericAttackEffect>().card = this;
        visualEffect.GetComponent<GenericAttackEffect>().dealEffect = true;
        yield return StartCoroutine(base.CardEffect());
    }

    public override void DealEffect()
    {
        effectFinished = true;
    }

    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseDamage = 25;// Deal damage
                thisVirtualCard.BaseDamage = 25;
                myWildCast.IncapacitatedDuration = 1;// Apply X incapacitate effects
                myWildCast.DamagePercentage = 0;// Deal X% base damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 32;// Deal damage
                myWildCast.IncapacitatedDuration = 1;// Apply X incapacitate effects
                thisVirtualCard.BaseDamage = 35;
                myWildCast.DamagePercentage = .2f;// Deal X% base damage
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 40;// Deal damage
                thisVirtualCard.BaseDamage = 40;
                myWildCast.IncapacitatedDuration = 2;// Apply X incapacitate effects
                myWildCast.DamagePercentage = .25f;// Deal X% base damage
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        if(myWildCast.DamagePercentage==0)
            thisVirtualCard.CardText.text = $"Deal {thisVirtualCard.CalculateAction(BaseDamage)} damage\nWildcast:\nIncapacitate {myWildCast.IncapacitatedDuration}";
        else
            thisVirtualCard.CardText.text = $"Deal {thisVirtualCard.CalculateAction(BaseDamage)} damage\nWildcast:\n Deal {thisVirtualCard.CalculateAction(BaseDamage)*myWildCast.DamagePercentage}\nIncapacitate {myWildCast.IncapacitatedDuration}";
    }
}
