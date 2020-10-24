﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;

public abstract class EnemyClass : MonoBehaviour
{
    #region Enemy Information
    [SerializeField] public EnemyData myData = new EnemyData(0,"",0,0,0,0,0);
    public Dictionary<string,EnemyAction> ActionList = new Dictionary<string, EnemyAction>();// List of Actions this Enemy has
    public List<EnemyAction> IntendedActions = new List<EnemyAction>();// Actions the enemy plan to execute
    [SerializeField] private GameObject AttackIcon=null;// Icon that will display that this enemy will attack
    [SerializeField] private GameObject ShieldIcon=null;// Icon that will display taht this enemy will defend/gain shield
    [SerializeField] private GameObject SpecialIcon=null;// Icon that wiill display that this enemy will use a special effect
    [SerializeField] private TMP_Text ShieldAmount = null;// Text that display the amount of shield the enemy has
    [SerializeField]protected float RandomValue;// This random value is rolled every end of turn and at start
    private Image HPBarFill;
    [Range(0,1)][SerializeField] protected float ShieldDecay=.5f;// The amount of shield lost at the start of every turn
    #endregion

    #region Events
    public Action OnEnemyGainShield;
    public Action OnEnemySpendShield;
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
    public void KillMe()// Instantly kill this enemy
    {
        DeathEvent?.Invoke();// Call the enemy's death event
        this.EnemyManager.RemoveEnemy(this);
        myDeath();// Destroy this enemy
    }
    #endregion

    #region Generic Combat Methods
    protected virtual void LoseLife(int Amount)// Reduce enemy HP 
    {
        myData.EnemyHP -= Amount;// Reduce enemy's HP by a given Amount
        myData.EnemyHP = myData.EnemyHP <= 0 ? 0 : myData.EnemyHP;// Check if HP is below zero
        HPBarFill.fillAmount = (float) myData.EnemyHP / myData.EnemyMaxHP;
        DeathConditionCheck();// Check if the enemy is dead;
    }
    protected virtual void GainLife(int Amount) => myData.EnemyHP += Amount;// Raises enemy HP
    public void GainShield(int ShieldAmount)// This function will modify the player's shield amount
    {
        // Any other methods that should be called when adding shield to the player's shield pool
        myData.EnemyShield += ShieldAmount;// Adds this amount to the player's shield pool
        OnEnemyGainShield?.Invoke();// Event called when enemy has gained shield
    }
    protected virtual int SpendShield(int Amount)// Spend an Amount of shield
    {
        int CurrentShield = myData.EnemyShield;// Current shield pool
        myData.EnemyShield -= Amount;// Reduce the pool by the amount of damage being applied
        myData.EnemyShield = myData.EnemyShield <= 0 ? 0 : myData.EnemyShield;// If the damage went beyond 0, set it to be 0, if not: keep the value
        OnEnemySpendShield?.Invoke();// Event called when enemy has spent shield
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
    private CombatPlayer combatplayer;
    public CombatPlayer Player
    {
        get
        {
            combatplayer = combatplayer ?? GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
            return combatplayer;
        }
    }
    #endregion

    #region Enemy Behaviour
    public abstract void EnemyIntention(); // Logic used by the enemy to determine its actions

    protected virtual void Awake()
    {
        OnEnemyGainShield += UpdateShield;
        OnEnemySpendShield += UpdateShield;
    }

    private void OnDisable()
    {
        OnEnemyGainShield -= UpdateShield;
        OnEnemySpendShield -= UpdateShield;
    }

    protected virtual void Start()
    {
        #region HP Bar Setup
        GameObject hpUI = Instantiate(Resources.Load("UI/EnemyUI/HP Bar Anchor"), EnemyManager.HPPositions[myData.Position], Quaternion.identity) as GameObject;
        hpUI.transform.SetParent(transform.Find("Enemy Canvas"),false);
        HPBarFill = hpUI.transform.Find("Bar Fill").GetComponent<Image>();
        HPBarFill.fillAmount = (float)myData.EnemyHP / myData.EnemyMaxHP;
        #endregion

        #region UI Update
        UpdateShield();
        #endregion

        RandomValue = UnityEngine.Random.value;// Initializes the RandomValue when this enemy spawns
        foreach (EnemyAction Action in GetComponents<EnemyAction>())// Go through all the EnemyAction components on this object
        {
            if(Action!=null)// If not null
                ActionList.Add(Action.ActionName,Action);// Adds each action on this gameobject to the dictionary
        }
        StartCoroutine(CheckIntention());// Check if this enemy changed its intention every .5 seconds
    }
    public virtual void StartTurn()
    {
        SpendShield((int)Mathf.Ceil((myData.EnemyShield * (ShieldDecay))));
    }
    public virtual void ActionPhase()
    {
        if (TurnManager.State == CombatState.EnemyActionPhase)
        {
            foreach (EnemyAction Action in IntendedActions)// Go through all the actions the enemy intends to perform
                if (Action != null)// Check if its null
                    Action.Effect();// Executes this action's effect
            EndTurn();// End its turn
        }
    }
    public virtual void EndTurn()
    {
        //Do a bunch of stuff
        RandomValue= UnityEngine.Random.value;// Generates a random value every end of turn
        EnemyManager.EndEnemyTurn();
    }
    IEnumerator CheckIntention()// Delays the intention check
    {
        while(this!=null)// While this script exists
        {
            EnemyIntention();// Checks what the enemy is going to do
            foreach(EnemyAction Action in IntendedActions)// Go through the enemy intended actions
                if(Action!=null)// If action is not null
                {
                    AttackIcon.SetActive(Action.myInfo.isAttack);// Activate using the type of action as a boolean
                    ShieldIcon.SetActive(Action.myInfo.isShield);// Activate using the type of action as a boolean
                    SpecialIcon.SetActive(Action.myInfo.isSpecial);// Activate using the type of action as a boolean
                    Action.ShowValue();// Show that skill damage value
                }
            yield return new WaitForSeconds(.5f);// Hold for .5 seconds
        }
    }
    #endregion

    #region UI Update
    public void UpdateShield()
    {
        ShieldAmount.text = "Shield : " + myData.EnemyShield;
    }
    #endregion

}
