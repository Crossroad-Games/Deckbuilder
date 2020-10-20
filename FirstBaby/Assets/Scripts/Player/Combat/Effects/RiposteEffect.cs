using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiposteEffect : MonoBehaviour
{
    private int BaseDamage = 20;// Damage value that will be applied to the Enemy
    private int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    private float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value

    private CombatPlayer player; //Reference to the comabt player

    private void Awake()
    {
        player = GetComponent<CombatPlayer>();
        player.OnPlayerProcessDamage += Riposte;
    }

    private void OnDisable()
    {
        player.OnPlayerProcessDamage -= Riposte;
    }

    public void InitializeRiposte(int AddValue, int SubtractValue, float Multiplier, float Divider)
    {
        this.AddValue = AddValue;
        this.SubtractValue = SubtractValue;
        this.Multiplier = Multiplier;
        this.Divider = Divider;
    }

    private void Riposte(EnemyClass attackingEnemy, int Damage)
    {
        attackingEnemy.ProcessDamage((Damage/2 + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        Destroy(this);
    }
}
