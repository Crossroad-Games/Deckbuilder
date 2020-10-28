using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransformation : EnemyAction
{
    // Start is called before the first frame update
    public GameObject EnemyTransformationTarget;// Reference to the enemy this enemy will turn into
    private EnemyClass ClassTransformationTarget;// Reference to the enemy class this enemy will have
    protected override void Start()
    {
        base.Start();
        
    }
    public override void Effect()
    {
        var EnemyToSpawn = (GameObject)Instantiate(EnemyTransformationTarget);// Instantiates the enemy
        ClassTransformationTarget = EnemyToSpawn.GetComponent<EnemyClass>();// Get its enemyclass
        ClassTransformationTarget.myData.Position = myClass.myData.Position;// Will spawn at the same position as this enemy
        EnemyToSpawn.transform.position = myClass.EnemyManager.EnemyPositions[ClassTransformationTarget.myData.Position];// This enemy will be sent to position it was first spawned on
        ClassTransformationTarget.GainShield(CalculateAction(myInfo.BaseShield));// Add some shield
        myClass.EnemyManager.AddEnemy(ClassTransformationTarget);// Add this enemy to the combat scene
        myClass.KillMe();// TEMPORARY! SHOULD DIE WITHOUT TRIGGERING DEATH EVENTS
    }
}
