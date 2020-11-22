using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicStrikeOverflow : OverflowKeyword
{
    [SerializeField] public float DamagePercentage=1f;// How much of the base damage will this overflow deal?
    public override void OverflowEffect()
    {
        foreach (EnemyClass Enemy in enemyManager.CombatEnemies)
        {
            //Instantiate attack visual effect
            targetEnemies.Clear();
            targetEnemies.Add(enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)]);// Pick a random enemy
            Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
            GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericAttackKeywordEffect/GenericAttackKeywordEffect"), playerSpriteTransform.position, Quaternion.identity) as GameObject;
            visualEffect.GetComponent<GenericAttackKeywordEffect>().targetTransform = Enemy.transform;
            visualEffect.GetComponent<GenericAttackKeywordEffect>().virtualCard = this.myCard;
            visualEffect.GetComponent<GenericAttackKeywordEffect>().dealEffect = false;
            if (Enemy == enemyManager.CombatEnemies[0])
            {
                visualEffect.GetComponent<GenericAttackKeywordEffect>().dealEffect = true;
                //Instantiate card UI
                GameObject canvas = GameObject.Find("Canvas");
                GameObject cardUI = Instantiate(Resources.Load("UI/Cards UI/" + myCard.cardInfo.ID), keywordCardUIPosition, Quaternion.identity, canvas.transform) as GameObject;
                visualEffect.GetComponent<GenericAttackKeywordEffect>().cardUI = cardUI;
                //-------------------------------- 
            }
        }
        
    }

    public override void DealEffect()
    {
        //Deal area damage
        foreach (EnemyClass Enemy in enemyManager.CombatEnemies)// Cycle through each enemy
            if (Enemy != null)// If enemy is not null
            {
                Debug.Log(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage * DamagePercentage)));
                Enemy.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage * DamagePercentage)));// Deal the base damage as overflow damage 
            }
    }
}
