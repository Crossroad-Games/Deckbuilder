using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardExtension : MonoBehaviour
{
    #region References
    protected PhysicalCard myCard;
    protected CombatPlayer combatPlayer;
    protected CombatManager combatManager;
    protected EnemyManager enemyManager;
    protected EnemyClass targetEnemy;
    #endregion
    [HideInInspector] public string Keyword;// This extension's keyword
    public abstract void ExtensionEffect();// Each Keyword will have its own effect
    protected virtual void Awake()
    {
        myCard = GetComponent<PhysicalCard>();
        combatPlayer = FindObjectOfType<CombatPlayer>();
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
        enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
    }
}
