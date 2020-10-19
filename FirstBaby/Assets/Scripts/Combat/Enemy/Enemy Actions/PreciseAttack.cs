using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseAttack : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseDamage = 6;
    [SerializeField] private float Multiplier = 1; // Modify this field to multiply damage
    [SerializeField] private float Divider = 1;// Modify this field to divide damage
    [SerializeField] private int AddedDamaged = 0;// Modify this field to add damage
    [SerializeField] private int SubtractedDamage = 0;// Modify this field to subtract damage
    void Awake()
    {
        BaseDamage = myInfo.BaseDamage;
    }
    public override void Effect()
    {
        var Damage = (int)Mathf.Ceil((myInfo.BaseDamage + AddedDamaged - SubtractedDamage) * (Multiplier / Divider));// Calculates the final damage
        if (Player.myData.PlayerShield == 0)// If the player doesnt have any shield
            Damage *= 2;// Double its damage
        Player.ProcessDamage(Damage);// Apply damage to the player
    }
}
