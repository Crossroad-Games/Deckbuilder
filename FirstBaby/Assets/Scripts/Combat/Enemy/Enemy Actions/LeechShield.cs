using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechShield : EnemyAction
{
    // Start is called before the first frame update
    [Header("Basic Information")]
    [SerializeField] private readonly int thisID = 1;
    [SerializeField] private readonly string thisName = "Leech Shield";
    [SerializeField] private readonly string thisDescription = "Steals some shield from the player";
    [Space(5)]
    [Header("Action values")]
    [SerializeField] private int BaseDamage = 10;
    [SerializeField] private float Multiplier = 1; // Modify this field to multiply damage
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Effect()
    {
        var ShieldDamage = (int)Mathf.Ceil((BaseDamage + AddedDamaged - SubtractedDamage) * (Multiplier / Divider));// Calculates the final shield leech
        var ShieldLeeched = (Player.myData.PlayerShield-ShieldDamage) < 0 ? 0 : ShieldDamage;// If there was nothing to leech, then don't gain any shield
        myClass.GainShield(ShieldLeeched);// Gain the amount of shield stolen from the player
    }
}
