﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class EnemyClass : MonoBehaviour
{
    #region Enemy Information
    [SerializeField] public EnemyData myData = new EnemyData();
    public Dictionary<string,EnemyAction> ActionList = new Dictionary<string, EnemyAction>();// List of Actions this Enemy has
    #endregion

    #region Death related methods
    public Action DeathEvent;
    public virtual void DeathConditionCheck()// Checks if the enemy's death condition was reached
    {
        if (myData.EnemyHP <= 0)// EnemyHP reached 0
        {
            DeathEvent?.Invoke();// Call the enemy's death event
            this.EnemyManager.RemoveEnemy(this);
            myDeath();// Destroy this enemy
        }
    }
    protected virtual void myDeath() => Destroy(this.gameObject);// When killed, destroy this gameobject
    #endregion

    #region Generic Combat Methods
    protected virtual void LoseLife(int Amount)// Reduce enemy HP 
    {
        myData.EnemyHP -= Amount;// Reduce enemy's HP by a given Amount
        myData.EnemyHP = myData.EnemyHP <= 0 ? 0 : myData.EnemyHP;// Check if HP is below zero
        DeathConditionCheck();// Check if the enemy is dead;
    }
    protected virtual void GainLife(int Amount) => myData.EnemyHP += Amount;// Raises enemy HP
    public void GainShield(int ShieldAmount)// This function will modify the player's shield amount
    {
        // Any other methods that should be called when adding shield to the player's shield pool
        myData.EnemyShield += ShieldAmount;// Adds this amount to the player's shield pool
    }
    protected virtual int SpendShield(int Amount)// Spend an Amount of shield
    {
        int CurrentShield = myData.EnemyShield;// Current shield pool
        myData.EnemyShield -= Amount;// Reduce the pool by the amount of damage being applied
        myData.EnemyShield = myData.EnemyShield <= 0 ? 0 : myData.EnemyShield;// If the damage went beyond 0, set it to be 0, if not: keep the value
        return CurrentShield;
    }
    public virtual void ProcessDamage(int Damage)// Modifies the incoming damage before applying it to the HP
    {
        Damage -= myData.EnemyDefense;// Reduce the damage by the enemy defense
        Damage -= SpendShield(Damage);// Spend the shield pool to reduce the incoming damage
        Damage = Damage <= 0 ? 0 : Damage;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(Damage);// Apply damage to the enemy's HP
    }
    #endregion

    #region References
    private EnemyManager enemymanager;
    public EnemyManager EnemyManager
    {
        get
        {
            enemymanager = enemymanager ?? GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
            return enemymanager;
        }
    }
    #endregion

    #region Enemy Behaviour
    public abstract void CombatLogic(); // Logic used by the enemy to determine its actions
    public virtual void Start()
    {
        foreach(EnemyAction Action in GetComponents<EnemyAction>())// Go through all the EnemyAction components on this object
        {
            if(Action!=null)// If not null
                ActionList.Add(Action.ActionName,Action);// Adds each action on this gameobject to the dictionary
        }
    }
    public virtual void StartTurn()
    {
        //Do a bunch of stuff

    }
    public virtual void ActionPhase()
    {
        if (TurnManager.State == CombatState.EnemyActionPhase)
        {
            CombatLogic();// Go through all of its combat logic
            EndTurn();// End its turn
        }
    }
   
    public virtual void EndTurn()
    {
        //Do a bunch of stuff
        EnemyManager.EndEnemyTurn();
    }
    #endregion

}
