using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastingEffect : MonoBehaviour
{
    protected int BaseValue = 20;// Damage value that will be applied to the Enemy
    protected int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    protected float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int turnCounter = 1;

    protected CombatPlayer player; //Reference to the comabt player


    protected virtual void Awake()
    {
        player = GetComponent<CombatPlayer>();
    }

    protected virtual void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void InitializeEffect(int BaseValue, int AddValue, int SubtractValue, float Multiplier, float Divider, int turnCounter)
    {
        this.BaseValue = BaseValue;
        this.AddValue = AddValue;
        this.SubtractValue = SubtractValue;
        this.Multiplier = Multiplier;
        this.Divider = Divider;
        this.turnCounter = turnCounter;
    }
    private void Countdown()
    {
        turnCounter--;
        if (turnCounter <= 0)
            Destroy(this);
    }
    public virtual void Effect(EnemyClass attackingEnemy, int Damage) //This overload is used when the effect is called when the player suffered damage
    {
        Countdown();
    }
    public virtual void Effect()
    {
        Countdown();
    }
    public virtual void AddStacks(int Amount)
    {
        turnCounter += Amount;
    }

    
}
