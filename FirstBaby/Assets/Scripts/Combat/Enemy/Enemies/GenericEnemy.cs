﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : EnemyClass
{
    #region Enemy Information
    // Start is called before the first frame update
    [Header("Enemy Information")]
    [SerializeField] private readonly int ID=-1;
    [SerializeField] private readonly string Name= string.Empty;
    [SerializeField] private readonly int InitialHP= 100;
    [SerializeField] private readonly int MaxHP= 100;
    [SerializeField] private readonly int Defense= 5;
    #endregion
    public override void Start()
    {
        base.Start();
        setEnemyAttributes(ID, name, InitialHP, MaxHP, Defense);// Sets the initial state of this Enemy
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CombatLogic()
    {
        ActionList["Enemy Attack"].Effect();// Use this action
    }

}