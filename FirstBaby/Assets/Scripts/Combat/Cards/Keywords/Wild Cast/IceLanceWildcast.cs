using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLanceWildcast : WildcastKeyword
{
    [SerializeField] public int IncapacitatedDuration = 1;
    [SerializeField] public float DamagePercentage = 0;// How much of the base damage is dealt when wild cast
    public override void WildCastEffect()
    {
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
        Debug.Log("Wildcast");
        targetEnemies[0].ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage * DamagePercentage)));// Deals a % of the base damage to the enemy
        IncapacitatedEffect preExistantEffect = targetEnemies[0].GetComponent<IncapacitatedEffect>();
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            IncapacitatedEffect effectToAdd = targetEnemies[0].gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Apply a incapacitated effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, IncapacitatedDuration);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(IncapacitatedDuration);// Extend the incapacitation effect
    }
}
