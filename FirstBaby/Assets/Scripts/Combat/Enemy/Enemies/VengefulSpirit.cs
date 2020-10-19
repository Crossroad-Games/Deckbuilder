using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulSpirit : EnemyClass
{

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CombatLogic()
    {
        if ((myData.EnemyHP + myData.EnemyShield) <= myData.EnemyMaxHP / 2)// If at or below 25% HP
            ActionList["Blood Ritual"].Effect();// Attack for double damage
        else if (Player.myData.PlayerShield == 0)// If the player does not have any shield
            ActionList["Precise Attack"].Effect();// Attack for double damage
        else
        {
            if(Random.value<=.25)
                ActionList["Enemy Attack"].Effect();// Attack and a small amount of damage
            else
                ActionList["Precise Attack"].Effect();// Attack for double damage
        }
    }
}
