using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : EnemyAction
{
    // Start is called before the first frame update
    public List<GameObject> EnemiesToSummon;// List of enemies that can be summoned with this action
    private EnemyClass whichEnemyClass;// Enemy class of the spawned enemy

    public override IEnumerator Effect()
    {
        var SummonedEnemy = (GameObject)Instantiate(EnemiesToSummon[Random.Range(0, EnemiesToSummon.Count)]);// Instantiates a random enemy from the list
        var PositionList = new List<int>() { 0, 1, 2, 3, 4 };// Possible positions on the scene
        whichEnemyClass = SummonedEnemy.GetComponent<EnemyClass>();// Acquires its enemy class
        foreach (EnemyClass Enemy in myClass.EnemyManager.CombatEnemies)// Cycle through all the enemies at the scene
            if (Enemy != null)// If Enemy is not null
                PositionList.Remove(Enemy.myData.Position);// Remove its position from the possible available
        whichEnemyClass.myData.Position = PositionList[0];// Spawns this enemy at the first available spot
        SummonedEnemy.transform.position = myClass.EnemyManager.EnemyPositions[whichEnemyClass.myData.Position];// This enemy will be sent to position it was first spawned on
        myClass.EnemyManager.AddEnemy(whichEnemyClass);// Add this enemy to the combat scene
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
