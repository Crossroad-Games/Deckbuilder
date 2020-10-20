using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornEffect : MonoBehaviour
{
    private int BaseDamage = 20;// Damage value that will be applied to the Enemy
    private int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    private float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value

    private CombatPlayer player; //Reference to the comabt player

    public ThornEffect(int BaseDamage, int AddValue, int SubtractValue, float Multiplier, float Divider)
    {
        this.BaseDamage = BaseDamage;
        this.AddValue = AddValue; 
        this.SubtractValue = SubtractValue;
        this.Multiplier = Multiplier; 
        this.Divider = Divider;
    }

    private void Awake()
    {
        player = GetComponent<CombatPlayer>();
        player.OnPlayerProcessDamage += Thorn;
    }

    private void OnDisable()
    {
        player.OnPlayerProcessDamage -= Thorn;
    }

    public void InitializeThorn(int BaseDamage, int AddValue, int SubtractValue, float Multiplier, float Divider)
    {
        this.BaseDamage = BaseDamage;
        this.AddValue = AddValue;
        this.SubtractValue = SubtractValue;
        this.Multiplier = Multiplier;
        this.Divider = Divider;
    }

    private void Thorn(EnemyClass attackingEnemy, int Damage)
    {
        attackingEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        Destroy(this);
    }
}
