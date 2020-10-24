using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastingEffect : MonoBehaviour
{
    protected int BaseDamage = 20;// Damage value that will be applied to the Enemy
    protected int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    protected float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int turnCounter = 1;

    protected CombatPlayer player; //Reference to the comabt player


    protected virtual void Awake()
    {
        player = GetComponent<CombatPlayer>();
        player.OnPlayerProcessDamage += Effect;
    }

    protected virtual void OnDisable()
    {
        player.OnPlayerProcessDamage -= Effect;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void InitializeRiposte(int AddValue, int SubtractValue, float Multiplier, float Divider, int turnCounter)
    {
        this.AddValue = AddValue;
        this.SubtractValue = SubtractValue;
        this.Multiplier = Multiplier;
        this.Divider = Divider;
        this.turnCounter = turnCounter;
    }

    public virtual void Effect(EnemyClass attackingEnemy, int Damage)
    {
        turnCounter--;
        if (turnCounter == 0)
            Destroy(this);
    }
}
