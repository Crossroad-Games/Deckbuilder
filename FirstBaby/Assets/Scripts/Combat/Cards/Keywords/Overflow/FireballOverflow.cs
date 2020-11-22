using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballOverflow : OverflowKeyword
{
    [SerializeField] public float DamagePercentage = .33f;// Deals 33% of the base damage
    public override void OverflowEffect()
    {
        //Instantiate attack visual effect
        targetEnemies.Clear();
        targetEnemies.Add(enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)]);// Pick a random enemy
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericAttackKeywordEffect/GenericAttackKeywordEffect"), playerSpriteTransform.position, Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().targetTransform = targetEnemies[0].transform;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().virtualCard = this.myCard;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().dealEffect = true;
        //-------------------------------- 

        //Instantiate card UI
        GameObject canvas = GameObject.Find("Canvas");
        GameObject cardUI = Instantiate(Resources.Load("UI/Cards UI/" + myCard.cardInfo.ID), keywordCardUIPosition, Quaternion.identity, canvas.transform) as GameObject;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().cardUI = cardUI;
        //--------------------------------
    }

    public override void DealEffect()
    {
        Debug.Log("Overflowing");
        targetEnemies[0].ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage * DamagePercentage)));
    }
}
