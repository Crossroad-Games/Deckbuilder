using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    // Start is called before the first frame update
    [Header("Basic Information")]
    [SerializeField] private readonly int thisID;
    [SerializeField] private readonly string thisName;
    [SerializeField] private readonly string thisDescription;
    [Space(5)]
    [Header("Action values")]
    [SerializeField] private int BaseDamage=5;
    [SerializeField] private float Multiplier=1; // Modify this field to multiply or divide the damage
    [SerializeField] private int AddedDamaged = 0;// Modify this field to add damage
    [SerializeField] private int SubtractedDamage = 0;// Modify this field to subtract damage
    void Start()
    {
        // Sets the information of ID, Name and Description of this Action //
        ActionID = thisID;
        ActionName = thisName;
        Description = thisDescription;
    }
    public override void Effect()
    {
        // Deal damage to the player
        var Damage = (int) Mathf.Ceil(BaseDamage * Multiplier) + AddedDamaged - SubtractedDamage;// Calculates the final damage
        throw new MissingReferenceException("Needs to have reference to the Player HP");
    }
}
