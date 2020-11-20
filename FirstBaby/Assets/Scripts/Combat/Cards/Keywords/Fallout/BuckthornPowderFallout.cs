using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowderFallout : FalloutKeyword
{
    [SerializeField] public int AmountofStacks=4;
    [SerializeField] public float DamageMultiplier=2f;
    //private GameObject cardUI;
    
    public override void FalloutEffect()
    {
        //Instantiate attack visual effect
        targetEnemies.Clear();
        targetEnemies.Add(enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)]);// Pick a random enemy
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericAttackKeywordEffect/GenericAttackKeywordEffect"), playerSpriteTransform.position, Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().targetTransform = targetEnemies[0].transform;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().virtualCard = this.myCard;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().dealEffect = true;
        Debug.Log("chamou fallout effect");

        //-------------------------------- 
        //Instantiate card UI
        GameObject canvas = GameObject.Find("Canvas");
        GameObject cardUI = Instantiate(Resources.Load("UI/Cards UI/" + myCard.cardInfo.ID), keywordCardUIPosition, Quaternion.identity, canvas.transform) as GameObject;
        visualEffect.GetComponent<GenericAttackKeywordEffect>().cardUI = cardUI;
        //--------------------------------
    }

    public override void DealEffect()
    {
        Agony preExistentAgony = targetEnemies[0].GetComponent<Agony>();
        if (preExistentAgony != null)
        {
            if (preExistentAgony.turnCounter >= 4) // agony >= 4 stacks, deal double damage
            {
                preExistentAgony.AddStacks(-4);
                targetEnemies[0].ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage * DamageMultiplier)));
            }
            else //otherwise, deal +4 agony
            {
                preExistentAgony.AddStacks(AmountofStacks);
            }
        }
        else //if there was no instance of agony on the enemy, add the component then initialize it
        {
            Agony effectToAdd = targetEnemies[0].gameObject.AddComponent<Agony>() as Agony;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);
        }
    }
}
