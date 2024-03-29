﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System.Linq;

public abstract class EnemyClass : MonoBehaviour
{
    #region Enemy Information
    [SerializeField] public EnemyData myData = new EnemyData(0,"",0,0,0,0,0);
    public Dictionary<string,EnemyAction> ActionList = new Dictionary<string, EnemyAction>();// List of Actions this Enemy has
    public List<EnemyAction> IntendedActions = new List<EnemyAction>();// Actions the enemy plan to execute
    protected List<EnemyAction> TurnActions = new List<EnemyAction>();// Copy of the actions during the action phase
    [SerializeField] private GameObject AttackIcon=null;// Icon that will display that this enemy will attack
    [SerializeField] private GameObject ShieldIcon=null;// Icon that will display taht this enemy will defend/gain shield
    [SerializeField] private GameObject SpecialIcon=null;// Icon that wiill display that this enemy will use a special effect
    private TMP_Text ShieldAmount = null;// Text that display the amount of shield the enemy has
    private TMP_Text HPAmount = null;// Text that display the amount of health the enemy has
    [SerializeField] protected float RandomValue;// This random value is rolled every end of turn and at start
    [SerializeField] public bool Incapacitated=false;// Is this enemy able to act?
    private List<SpriteRenderer> WardOverloadList = new List<SpriteRenderer>();
    private Vector3 UIPosition = new Vector3(-.07f,-.09f,0f);
    private GameObject HPBarFill;
    private GameObject WardBarFill = null;
    public IEnumerator CheckIntentionCoroutine;
    [Range(0,1)][SerializeField] public float ShieldDecay=.5f;// The amount of shield lost at the start of every turn
    #endregion

    #region Events
    public Action OnEnemyGainShield;
    public Action OnEnemySpendShield;
    public Action thisEnemyStartTurn;// An event tied specifically to this enemy
    public Action thisEnemyEndTurn;// An event tied specifically to this enemy
    #endregion

    #region Death related methods
    public Action<EnemyClass> DeathEvent;
    public virtual void DeathConditionCheck()// Checks if the enemy's death condition was reached
    {
        if (myData.EnemyHP <= 0)// EnemyHP reached 0
        {
            DeathEvent?.Invoke(this);// Call the enemy's death event
            this.EnemyManager.RemoveEnemy(this);
            myDeath();// Destroy this enemy
        }
    }
    private void Update()
    {
#if Unity_Editor
        if (Input.GetKeyDown(KeyCode.K))
            KillMe();
#endif
    }
    protected virtual void myDeath() => Destroy(this.gameObject);// When killed, destroy this gameobject
    public void KillMe()// Instantly kill this enemy
    {
        DeathEvent?.Invoke(this);// Call the enemy's death event
        this.EnemyManager.RemoveEnemy(this);
        myDeath();// Destroy this enemy
    }
    public void RemoveMe()// Instantly removes this enemy from combat without triggering death events
    {
        Debug.Log("Removed this enemy: " + this.gameObject.name);
        this.EnemyManager.RemoveEnemy(this);
        myDeath();
    }
#endregion

#region Generic Combat Methods
    protected virtual void LoseLife(int Amount)// Reduce enemy HP 
    {
        myData.EnemyHP -= Amount;// Reduce enemy's HP by a given Amount
        myData.EnemyHP = myData.EnemyHP <= 0 ? 0 : myData.EnemyHP;// Check if HP is below zero
        var X= (float)myData.EnemyHP / myData.EnemyMaxHP;
        HPBarFill.transform.localScale = new Vector3(X, HPBarFill.transform.localScale.y, HPBarFill.transform.localScale.z);
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
        if (Amount <= 0)
            return CurrentShield;
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
        HPAmount.text = myData.EnemyHP.ToString();
        //TODO:Update UI
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
        var TempObj = new GameObject();// Instantiates a new empty gameobject
        TempObj.name = "WardTextAnchor";// Rename it
        TempObj.transform.SetParent(this.transform);// Set its parent to be the enemy object
        var ChildTempObj = new GameObject();// Instantiates a new empty gameobject
        ChildTempObj.name = "WardValue";// Rename it
        ShieldAmount = ChildTempObj.AddComponent<TextMeshPro>();// Add a TMP_Text Asset and set its reference to be manipulated at a later time
        ShieldAmount.fontSize = 2;
        ChildTempObj.transform.SetParent(TempObj.transform);// Set its parent to be the Temp Obj
        TempObj.transform.localPosition = new Vector3(.985f, -.35f, 0);// Text position
        
#region HP Bar Setup
        GameObject hpUI = Instantiate(Resources.Load("UI/EnemyUI/HP Bar Anchor"), UIPosition, Quaternion.identity) as GameObject;
        hpUI.transform.SetParent(this.transform, false);
        HPBarFill = hpUI.transform.Find("Bar Fill").gameObject;
        HPBarFill.transform.localScale = new Vector3((float)myData.EnemyHP / myData.EnemyMaxHP, HPBarFill.transform.localScale.y, HPBarFill.transform.localScale.z);
        HPAmount = hpUI.GetComponentInChildren<TMP_Text>();
        HPAmount.text = myData.EnemyHP.ToString();
#endregion
#region Ward Bar Setup
        GameObject WardUI = Instantiate(Resources.Load("UI/EnemyUI/Ward Bar Anchor"), UIPosition, Quaternion.identity) as GameObject;
        WardUI.transform.SetParent(this.transform, false);
        WardOverloadList = WardUI.transform.Find("Ward Overloads").GetComponentsInChildren<SpriteRenderer>().ToList();// Converts all images on the children to a list of images
        foreach (SpriteRenderer Overload in WardOverloadList)// For each ward overload attached to the enemy
            if (Overload != null)// Check if null
                Overload.enabled = false;// Disable it
        WardBarFill = WardUI.transform.Find("Bar Fill").gameObject;
        UpdateShield();// Update this enemy's shield information
#endregion

        RandomValue = UnityEngine.Random.value;// Initializes the RandomValue when this enemy spawns 
        CheckIntentionCoroutine = CheckIntention();
        StartChecking();
    }
    protected void OnEnable()
    {
        foreach (EnemyAction Action in GetComponents<EnemyAction>())// Go through all the EnemyAction components on this object
        {
            if (Action != null)// If not null
                ActionList.Add(Action.ActionName, Action);// Adds each action on this gameobject to the dictionary
        }
    }
    public virtual void StartTurn()
    {
        thisEnemyStartTurn?.Invoke();
        SpendShield((int)Mathf.Ceil((myData.EnemyShield * (ShieldDecay))));    
    }
    public virtual void StopChecking() => StopCoroutine(CheckIntentionCoroutine);// Check if this enemy changed its intention every .5 seconds
    public virtual void StartChecking() => StartCoroutine(CheckIntentionCoroutine);// Check if this enemy changed its intention every .5 seconds
    public virtual void ActionPhase() => StartCoroutine(ActionPhaseCoroutine());
    public virtual void EndTurn()
    {
        //Do a bunch of stuff
        RandomValue= UnityEngine.Random.value;// Generates a random value every end of turn
        thisEnemyEndTurn?.Invoke();// Invoke all methods subscribed to this event
        EnemyManager.EndEnemyTurn();
    }
    IEnumerator CheckIntention()// Delays the intention check
    {
        while (this!=null)// While this script exists
        {
            if (!Incapacitated)// If this enemy can act
                EnemyIntention();// Checks what the enemy is going to do
            else if (IntendedActions.Count != 0)
                IntendedActions.Clear();
            if (!Incapacitated)// If this enemy can act
            {
                foreach (EnemyAction Action in IntendedActions)// Go through the enemy intended actions
                    if (Action != null)// If action is not null
                    {
                        AttackIcon.SetActive(Action.myInfo.isAttack);// Activate using the type of action as a boolean
                        ShieldIcon.SetActive(Action.myInfo.isShield);// Activate using the type of action as a boolean
                        SpecialIcon.SetActive(Action.myInfo.isSpecial);// Activate using the type of action as a boolean
                        Action.ShowValue();// Show that skill damage value
                    }
            }
            else// If this enemy can't act
            {
                AttackIcon.SetActive(false);// Deactivate using the type of action as a boolean
                ShieldIcon.SetActive(false);// Deactivate using the type of action as a boolean
                SpecialIcon.SetActive(false);// Deactivate using the type of action as a boolean
                GetComponentInChildren<TMP_Text>().text = string.Empty;// Show no value
            }
            yield return new WaitForSeconds(.5f);// Hold for .5 seconds
        }
    }
#endregion
    public virtual IEnumerator ActionPhaseCoroutine()
    {
        var ActionDone = false;
        AttackIcon.SetActive(false);// Deactivate using the type of action as a boolean
        ShieldIcon.SetActive(false);// Deactivate using the type of action as a boolean
        SpecialIcon.SetActive(false);// Deactivate using the type of action as a boolean
        GetComponentInChildren<TMP_Text>().text = string.Empty;// Show no value
        StopChecking();// Stop checking for intention until the end of the enemy phase
        if (TurnManager.State == CombatState.EnemyActionPhase)
        {
            if (!Incapacitated)// If not incapacitaded
                EnemyIntention();// Checks what the enemy is going to do
            else
            {
                IntendedActions.Clear();// Clear intended action
                ActionDone = true;
            }
            TurnActions = new List<EnemyAction>(IntendedActions);// Copy the actions this enemy is about to execute to be accessed after the action phase
            foreach (EnemyAction Action in TurnActions)// Go through all the actions the enemy intends to perform
                if (Action != null && !Incapacitated)// Check if its null
                    StartCoroutine(Action.Effect());// Executes this action's effect
            while (!ActionDone)// Hold the combat turn system until this action phase is over
            {
                foreach (EnemyAction Action in TurnActions)// Check each action that were used this turn by this enemy
                    if (Action.ActionDone)// If this action has already done its effect
                        ActionDone = true;// Action effect is over
                    else// If any action is still supposed to do its effect
                    {
                        ActionDone = false;// Action is not over
                        break;// Stop verifying
                    }
                yield return null;
            }
            foreach (EnemyAction Action in TurnActions)// Cycle through all the actions this enemy has
                Action.ActionDone = false;// Change all the action states to be not done   
            EndTurn();// End its turn
        }
    }
#region UI Update
    public void UpdateShield()
    {
        if (WardBarFill != null)
        {
            var X = 0f;
            if (myData.EnemyShield >= 100)// If there is more than 100 Ward
            {

                X = (myData.EnemyShield % 100) != 0 ? ((myData.EnemyShield % 100) / 100f) : 1;// Check if it is a multiple of 100, because 200 should be 100 on the bar and 1 Charge
                WardBarFill.transform.localScale = new Vector3(X, WardBarFill.transform.localScale.y, WardBarFill.transform.localScale.z);
                ShieldAmount.text = (myData.EnemyShield % 100) != 0 ? $"{(myData.EnemyShield % 100)}" : "100";// Uses the variable as a string
                for (var iterator = 0; iterator < WardOverloadList.Count; iterator++)// Go through all ward overload charges
                    WardOverloadList[iterator].enabled = iterator + 1 <= (Mathf.FloorToInt(myData.EnemyShield / 100));//  Enable all below the threshold(multiples of 100) and disable all above it
            }
            else
            {
                X = (float)myData.EnemyShield / 100f;// If there is less than 100 shield, convert it directly to %
                WardBarFill.transform.localScale = new Vector3(X, WardBarFill.transform.localScale.y, WardBarFill.transform.localScale.z);
                ShieldAmount.text = $"{myData.EnemyShield}";// Uses the variable as a string
                foreach (SpriteRenderer OverloadCharge in WardOverloadList)// Cycle through all overload charge sprites
                    if (OverloadCharge != null)// If it is not null
                        OverloadCharge.enabled = false;// Disable it
            }
        }
    }
#endregion

}
