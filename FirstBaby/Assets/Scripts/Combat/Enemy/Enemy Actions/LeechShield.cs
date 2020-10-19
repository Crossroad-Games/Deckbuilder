using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechShield : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseDamage = 10;
    [SerializeField] private float Multiplier = 1; // Modify this field to multiply damage
    [SerializeField] private float Divider = 1;// Modify this field to divide damage
    [SerializeField] private int AddedDamaged = 0;// Modify this field to add damage
    [SerializeField] private int SubtractedDamage = 0;// Modify this field to subtract damage
    void Awake()
    {
        BaseDamage = myInfo.BaseDamage;// Copies the value of the action info
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Effect()
    {
        var ShieldDamage = (int)Mathf.Ceil((myInfo.BaseDamage + AddedDamaged - SubtractedDamage) * (Multiplier / Divider));// Calculates the final shield leech
        var ShieldLeeched = (Player.myData.PlayerShield-ShieldDamage) < 0 ? 0 : ShieldDamage;// If there was nothing to leech, then don't gain any shield
        myClass.GainShield(ShieldLeeched);// Gain the amount of shield stolen from the player
    }
}
