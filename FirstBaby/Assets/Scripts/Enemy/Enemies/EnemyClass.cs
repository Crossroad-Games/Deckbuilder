using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class EnemyClass : MonoBehaviour
{
    #region Enemy Information
    public int ID { get; protected set; }// Unique identifier of this Enemy
    public string EnemyName { get; protected set; }
    public int EnemyHP { get; protected set; } // Current HP
    public int EnemyMaxHP { get; protected set; }// Maximum HP
    public int EnemyDefense { get; protected set; } // Constant removed from incoming damage
    public Dictionary<string,EnemyAction> ActionList = new Dictionary<string, EnemyAction>();// List of Actions this Enemy has
    // Function designed to set the initial state of all atributes
    public void setEnemyAttributes(int newID, string newEnemyName, int newEnemyHP, int newEnemyMaxHP, int newEnemyDefense)
    {
        ID = newID;
        EnemyName = newEnemyName;
        EnemyHP = newEnemyHP;
        EnemyMaxHP = newEnemyMaxHP;
        EnemyDefense = newEnemyDefense;
        DeathEvent += myDeath;

    }
    #endregion

    #region Death related methods
    public Action DeathEvent;
    public virtual void DeathConditionCheck()// Checks if the enemy's death condition was reached
    {
        if (EnemyHP == 0)// EnemyHP reached 0
            DeathEvent?.Invoke();// Call the enemy's death event
    }
    protected virtual void myDeath() => Destroy(this.gameObject);// When killed, destroy this gameobject
    #endregion

    #region Generic Methods
    protected virtual void LoseLife(int Amount) => EnemyHP -= Amount;// Reduce enemy HP 
    protected virtual void GainLife(int Amount) => EnemyHP += Amount;// Raises enemy HP
    public virtual void ProcessDamage(int Damage)
    {
        Damage = Damage - EnemyDefense;// Reduce the damage by the enemy defense
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
        EnemyManager.AddEnemy(this);
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
