using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MischievousSpirit : EnemyClass
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CombatLogic()
    {
        if (myData.Position == 0 && (EnemyManager.CombatEnemies.Count) >= 3)// If there are 3 enemies and this is the first one: Steal the player shield
        {
            Debug.Log("Leeched");
            ActionList["Leech Shield"].Effect();// Use this action 
        }
        else if (Random.value <= .7)
        {
            Debug.Log("Attacked");
            ActionList["Enemy Attack"].Effect();// Use this action 
        }
        else
        {
            Debug.Log("Leeched");
            ActionList["Leech Shield"].Effect();// Use this action 
        }
    }
}
