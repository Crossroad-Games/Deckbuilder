﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyAction : MonoBehaviour
{
    public int ActionID { get { return myInfo.thisID; } protected set { } }// Unique identifier of this Action
    public string ActionName { get { return myInfo.thisName; } protected set { } }
    public string Description { get { return myInfo.thisDescription; } protected set { } }
    public int AddValue = 0, SubtractValue = 0;
    public float Multiplier = 1, Divider = 1;
    private EnemyClass myclass;
    public bool ActionDone = false;// Wheter this action is done doing its effect and animation or not
    public EnemyClass myClass// Keeps reference to which Enemy Class has ownership over this script
    {
        get
        {
            myclass = myclass ?? gameObject.GetComponent<EnemyClass>();// If this field is currently null, get the reference from the object attached to
            return myclass;// Return the reference
        }
    }
    private CombatPlayer player;
    public CombatPlayer Player
    {
        get
        {
            player = player ?? GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Sets reference to the player object
            return player;
        }
    }
    private TMP_Text actionvalue;
    public TMP_Text ActionValueText
    {
        get
        {
            actionvalue = actionvalue ?? transform.Find("Action Value").GetComponent<TMP_Text>();
            return actionvalue;
        }
    }
    public virtual void ShowValue()// Display a numeric value that will represent the potency of this attack
    {
        if(myInfo.isAttack)// If it is an attack action
            ActionValueText.text = myInfo.BaseDamage <= 0 ? "?": $"{CalculateAction(myInfo.BaseDamage)}";// Show its damage, if there is no base damage show a question mark
        else if(myInfo.isShield)
            ActionValueText.text = myInfo.BaseShield<=0? "?": $"{CalculateAction(myInfo.BaseShield)}";// Show its extra shield, if there is no base shield show a question mark
    }
    protected virtual int CalculateAction(int ActionValue)
    {
        return (int)Mathf.Ceil((ActionValue + AddValue - SubtractValue) * (Multiplier / Divider));// Calculates the final damage
    }
    protected virtual void Start()
    {
        #region Enemy Info setup
        var TempInfo = Object.Instantiate(myInfo);
        myInfo = TempInfo;
        #endregion
        if (Customizable)// If an enemy requires a slightly different inspector
        {
            if (CustomDamage)// Customize this action's damage
                myInfo.BaseDamage =(int)(myInfo.BaseDamage*BaseDamageMultiplier);
            if (CustomShield)// Customize this action's shield
                myInfo.BaseShield = (int)(myInfo.BaseShield*BaseShieldMultiplier);
        }
    }
    [SerializeField] public EnemyActionInfo myInfo;
    [SerializeField] public bool Customizable, CustomDamage, CustomShield;
    [SerializeField] public float BaseDamageMultiplier, BaseShieldMultiplier;
    public abstract IEnumerator Effect();
}
