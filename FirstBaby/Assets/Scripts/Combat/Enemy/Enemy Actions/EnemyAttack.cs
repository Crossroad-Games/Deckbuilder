using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    // Start is called before the first frame update
    [Header("Basic Information")]
    [SerializeField] private readonly int thisID= 0;
    [SerializeField] private readonly string thisName= "Enemy Attack";
    [SerializeField] private readonly string thisDescription= "Most basic attack type, deals (Base Damage +- Modifiers)*Scale damage";
    [Space(5)]
    [Header("Action values")]
    [SerializeField] private int BaseDamage=5;
    [SerializeField] private float Multiplier=1; // Modify this field to multiply damage
    [SerializeField] private float Divider = 1;// Modify this field to divide damage
    [SerializeField] private int AddedDamaged = 0;// Modify this field to add damage
    [SerializeField] private int SubtractedDamage = 0;// Modify this field to subtract damage
    void Awake()
    {
        // Sets the information of ID, Name and Description of this Action //
        ActionID = thisID;
        ActionName = thisName;
        Description = thisDescription;
    }
    public override void Effect()
    {
        // Deal damage to the player
        var Damage = (int) Mathf.Ceil((BaseDamage + AddedDamaged - SubtractedDamage)*(Multiplier/Divider));// Calculates the final damage
        Player.ProcessDamage(Damage);// Apply damage to the player
        
    }
}
