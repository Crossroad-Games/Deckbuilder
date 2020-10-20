using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyAction", menuName = "Data/EnemyAction/Action Info")]
public class EnemyActionInfo : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] public  int thisID = -1;
    [SerializeField] public string thisName = "Default";
    [SerializeField] public string thisDescription = "Default";
    [SerializeField] public bool isAttack, isShield, isSpecial;
    [Space(5)]
    [Header("Attack value")]
    [SerializeField] public int BaseDamage = 5;
    [Header("Shield value")]
    [SerializeField] public int BaseShield = 0;
}
