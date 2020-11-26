using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiptideCard : TargetCard
{
    private HastenKeyword myHasten;// Reference to the Hasten keyword will be used on the LevelRanks method to determine how strong will the hasten effect be
    private RiptideOverflow myOverflow;// Reference to the overflow keyword will be used on the levelranks method to determine how strong will the overflow effect be
    protected override void Awake()
    {
        myHasten = GetComponent<HastenKeyword>();// Reference is set
        myOverflow = GetComponent<RiptideOverflow>();// Reference is set
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
                BaseDamage = 5;// Deal damage
                myHasten.HastenAmount = 1;// Hasten X
                myOverflow.ExtraDamage = 5;// Deal +X Damage
                break;
            case 1:// One LVL higher than base
                BaseDamage = 5;// Deal damage
                myHasten.HastenAmount = 2;// Hasten X
                myOverflow.ExtraDamage = 7;// Deal +X Damage
                break;
            case 2:// Two LVLs higher than base
                BaseDamage = 5;// Deal damage
                myHasten.HastenAmount = 3;// Hasten X
                myOverflow.ExtraDamage = 10;// Deal +X Damage
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Unstable 3\nHasten {myHasten.HastenAmount}\nDeal {thisVirtualCard.CalculateAction(BaseDamage)} damage\nOverflow:\n+{myOverflow.ExtraDamage} damage";
    }
}
