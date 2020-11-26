using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicStrikeCard : TargetCard
{
    [SerializeField] private int IncapacitatedDuration = 3;
    private SeismicStrikeOverflow myOverflow;// Reference to the overflow keyword will be used to determine how strong will the overflow effect be
    protected override void Awake()
    {
        myOverflow = GetComponent<SeismicStrikeOverflow>();// Reference is defined
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
        IncapacitatedEffect preExistantEffect = TargetEnemy.GetComponent<IncapacitatedEffect>();
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            IncapacitatedEffect effectToAdd = TargetEnemy.gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Apply a incapacitated effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, IncapacitatedDuration);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(IncapacitatedDuration);
    }

    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseDamage = 5;// Deal damage
                thisVirtualCard.BaseDamage = 5;
                myOverflow.DamagePercentage = 1;// Deal X% damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 8;// Deal damage
                thisVirtualCard.BaseDamage = 8;
                myOverflow.DamagePercentage = 1.5f;// Deal X% damage
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 12;// Deal damage
                thisVirtualCard.BaseDamage = 12;
                myOverflow.DamagePercentage = 2;// Deal X% damage
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Unstable 3\nDeal {thisVirtualCard.CalculateAction(BaseDamage)} damage\nIncapacitate 3\nOverflow:\nDeal {thisVirtualCard.CalculateAction(BaseDamage)*myOverflow.DamagePercentage} damage to ALL enemies";
    }
}
