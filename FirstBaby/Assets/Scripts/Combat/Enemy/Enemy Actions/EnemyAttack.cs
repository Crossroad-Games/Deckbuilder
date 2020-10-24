using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    // Start is called before the first frame update
    [Header("Action values")]
    [SerializeField] private int BaseDamage=5;
    private void Start()
    {
        BaseDamage = myInfo.BaseDamage;
    }
    public override void Effect()
    {
        // Deal damage to the player
        var Damage = CalculateAction(myInfo.BaseDamage);
        Player.ProcessDamage(myClass,Damage);// Apply damage to the player
        
    }
    
}   
