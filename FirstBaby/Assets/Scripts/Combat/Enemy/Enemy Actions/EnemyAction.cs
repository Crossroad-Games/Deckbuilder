using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyAction : MonoBehaviour
{
    public int ActionID { get { return myInfo.thisID; } protected set { } }// Unique identifier of this Action
    public string ActionName { get { return myInfo.thisName; } protected set { } }
    public string Description { get { return myInfo.thisDescription; } protected set { } }
    public abstract void Effect();// Every Action must have an Effect
    protected int AddValue=0, SubtractValue=0;
    protected float Multiplier = 1, Divider=1;
    private EnemyClass myclass;
    public EnemyClass myClass// Keeps reference to which Enemy Class has ownership over this script
    {
        get
        {
            myclass = myclass?? gameObject.GetComponent<EnemyClass>();// If this field is currently null, get the reference from the object attached to
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
    public virtual void ShowValue()// Display a numeric value that will represent the potency of this attack
    {
        if(myInfo.isAttack)// If it is an attack action
            GetComponentInChildren<TMP_Text>().text = myInfo.BaseDamage <= 0 ? "?": $"{CalculateAction(myInfo.BaseDamage)}";// Show its damage, if there is no base damage show a question mark
        else if(myInfo.isShield)
            GetComponentInChildren<TMP_Text>().text = myInfo.BaseShield<=0? "?": $"{CalculateAction(myInfo.BaseShield)}";// Show its extra shield, if there is no base shield show a question mark
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
}
