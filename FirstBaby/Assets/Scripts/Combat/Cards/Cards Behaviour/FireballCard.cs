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
                BaseDamage = 18;// Deal damage
                thisVirtualCard.BaseDamage = 18;
                myOverflow.DamagePercentage = .33f;// Deal % of base damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 28;// Deal damage
                thisVirtualCard.BaseDamage = 28;
                myOverflow.DamagePercentage = .33f;// Deal % of base damage
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 36;// Deal damage
                thisVirtualCard.BaseDamage = 36;
                myOverflow.DamagePercentage = .5f;// Deal % of base damage
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Deal {thisVirtualCard.CalculateAction(BaseDamage)} damage\nOverflow:\nDeal {Mathf.CeilToInt(thisVirtualCard.CalculateAction(BaseDamage)*myOverflow.DamagePercentage)} damage";
    }
}
