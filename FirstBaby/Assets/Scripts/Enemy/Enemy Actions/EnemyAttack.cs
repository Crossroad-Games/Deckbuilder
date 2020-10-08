using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(myClass);
    }
    public override void Effect()
    {
        // Deal damage to the player
        throw new MissingReferenceException("Needs to have reference to the Player HP");
    }
}
