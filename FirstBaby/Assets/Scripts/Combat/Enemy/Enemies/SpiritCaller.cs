using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpiritCaller : EnemyClass
{
    private int AmountToSummon;// How many enemies this enemy plans to summon
    [SerializeField] private float GuardianBuff = .1f;// How much shield depletion each guardian removes while alive
    [SerializeField] private int GuardianShieldGain = 15;// How much shield this enemy gains when summoning a Guardian Spirit
    [SerializeField] private int MischievousBuff = 1;// How much defense each mischievous spirit adds to this while alive
    [SerializeField] private int VengefulBuff = 5;// How much extra damage this enemy deals per vengeful spirit alive
    [SerializeField] private int SummonCD = 4;// CD between each summoning wave
    [SerializeField] private int CurrentSummonCD = 0;// Current CD for summoning
    private List<EnemyClass> EnemyList;// List of enemies spawned and alive which this enemy has an event subscription
    #region Startup and Death
    protected override void Start()
    {
        base.Start();
        EnemyList = new List<EnemyClass>();// Initializes a new empty list
        foreach (EnemyClass Enemy in EnemyManager.CombatEnemies)// Cycle through all the enemies at the moment this enemy spawns
            if (Enemy != null)// If enemy is not null
                HandleSpiritSpawn(Enemy);// Check if it is a spirit and handle it
        EnemyManager.OnSpawnEnemy += HandleSpiritSpawn;// Subscribe to the enemy spawn event
    }
    private void OnDisable()
    {
        EnemyManager.OnSpawnEnemy -= HandleSpiritSpawn;// Unsubscribe to the enemy spawn event
    }
    #endregion

    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if (EnemyManager.CombatEnemies.Count <= 3 && CurrentSummonCD <= 0)// If there are 3 or less enemies
        {
            AmountToSummon = 5 - EnemyManager.CombatEnemies.Count;// Summons enough to populate the whole scene
            for (var i = AmountToSummon; i > 0; i--)// Call once for every slot
                IntendedActions.Add(ActionList["Summon Enemy"]);// Add a spawn action
        }
        else if (RandomValue <= .6)
            IntendedActions.Add(ActionList["Enemy Attack"]);// Attack the player
        else if(RandomValue<=1)
            IntendedActions.Add(ActionList["Protection"]);// Gain shield
    }
    public override void ActionPhase()
    {
        base.ActionPhase();
        foreach (EnemyAction Action in IntendedActions)// Go through all the actions the enemy intends to perform
            if (Action != null)// Check if its null
            {
                if (Action.ActionName == "Summon Enemy")// If this enemy used Summon Enemy this turn
                    CurrentSummonCD = SummonCD;// Apply CD
            }
    }
    public override void StartTurn()
    {
        base.StartTurn();
        CurrentSummonCD = CurrentSummonCD > 0 ? CurrentSummonCD-- : 0;// If greater than 0, reduce, if not equals to 0
    }
    #region HandleSummons
    public void HandleSpiritSpawn(EnemyClass SpawnedEnemy)// Acquires the enemy that just got spawned
    {
        var EnemyName = SpawnedEnemy.myData.EnemyName;// Acquires that enemy's name
        switch(EnemyName)// Check if the enemy that just spawned should trigger an effect
        {
            case "Guardian Spirit":
                ShieldDecay -= GuardianBuff;// Removes some shield decay for each guardian spirit
                EnemyList.Add(SpawnedEnemy);// Keeps reference to this enemy until it dies
                GainShield(GuardianShieldGain);// Gain some shield when summoning a Guardian Spirit
                SpawnedEnemy.DeathEvent += RemoveGuardianBuff;// When this enemy dies, call this method
                break;
            case "Mischievous Spirit":
                myData.EnemyDefense += MischievousBuff;// Adds some defense for each mischievous spirit
                EnemyList.Add(SpawnedEnemy);// Keeps reference to this enemy until it dies
                SpawnedEnemy.DeathEvent += RemoveMischievousBuff;// When this enemy dies, call this method
                break;
            case "Vengeful Spirit":
                ActionList["Enemy Attack"].AddValue += VengefulBuff;// Adds some damage for each Vengeful spirit alive
                SpawnedEnemy.GetComponent<BloodRitual>().ImmuneNames.Add(myData.EnemyName);// Spirit caller is immune to Blood Ritual
                EnemyList.Add(SpawnedEnemy);// Keeps reference to this enemy until it dies
                SpawnedEnemy.DeathEvent += RemoveVengefulBuff;// When this enemy dies, call this method
                break;
            default:
                Debug.Log(SpawnedEnemy.myData.EnemyName);
                break;
        }
    }
    public void RemoveEnemyList(EnemyClass EnemyToRemove) => EnemyList.Remove(EnemyToRemove);// Removes this enemy from this list if it dies
    public void RemoveGuardianBuff(EnemyClass Enemy) => ShieldDecay += GuardianBuff;// Returns to the proper amount
    public void RemoveMischievousBuff(EnemyClass Enemy) => myData.EnemyDefense -= MischievousBuff;// Returns to the proper amount
    public void RemoveVengefulBuff(EnemyClass Enemy) => ActionList["Enemy Attack"].AddValue -= VengefulBuff;// Returns to the proper amount
    #endregion
}
