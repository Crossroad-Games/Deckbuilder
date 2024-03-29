﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySingleEffect : EnemyEffect
{
    [SerializeField] protected int BaseValue = 0;// Damage value that will be applied to the Player
    protected int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    protected float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int turnCounter = 1;

    protected CombatPlayer player; //Reference to the comabt player
    protected EnemyClass myClass;// Reference to the enemyclass

    protected virtual void Awake()
    {
        player = GetComponent<CombatPlayer>();
        myClass = GetComponent<EnemyClass>();
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

    public virtual void Effect(EnemyClass attackingEnemy, int Damage)
    {
        Destroy(this);
    }

    public virtual void Effect()
    {
        Destroy(this);
    }
}
