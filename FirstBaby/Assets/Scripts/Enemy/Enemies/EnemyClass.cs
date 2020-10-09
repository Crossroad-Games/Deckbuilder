using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class EnemyClass : MonoBehaviour
{
    // Enemy information //
    public int ID { get; protected set; }// Unique identifier of this Enemy
    public string EnemyName { get; protected set; }
    public int EnemyHP { get; protected set; } // Current HP
    public int EnemyMaxHP { get; protected set; }// Maximum HP
    public int EnemyDefense { get; protected set; } // Constant removed from incoming damage
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

    // Enemy Death Condition and Event //
    public Action DeathEvent;
    public virtual void DeathConditionCheck()// Checks if the enemy's death condition was reached
    {
        if (EnemyHP == 0)// EnemyHP reached 0
            DeathEvent?.Invoke();// Call the enemy's death event
    }

    // Enemy generic methods //
    protected virtual void LoseLife(int Amount) => EnemyHP -= Amount;// Reduce enemy HP 
    protected virtual void GainLife(int Amount) => EnemyHP += Amount;// Raises enemy HP
    protected virtual void myDeath() => Destroy(this.gameObject);// When killed, destroy this gameobject
    public virtual void ProcessDamage(int Damage)
    {
        Damage = Damage - EnemyDefense;// Reduce the damage by the enemy defense
        Damage = Damage <= 0 ? 0 : Damage;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(Damage);// Apply damage to the enemy's HP
    }

    // Enemy Behaviour //
    public abstract void CombatLogic(); // Logic used by the enemy to determine its actions
    public virtual void Awake()
    {
        
    }

}
