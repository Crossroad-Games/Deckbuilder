using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyLastingEffect : EnemyEffect
{
    [SerializeField]protected int BaseValue = 0;// Damage value that will be applied to the Player
    protected int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    protected float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int turnCounter = 1;

    protected CombatPlayer player; //Reference to the comabt player
    protected EnemyClass myClass;// Reference to the enemyclass
    protected TMP_Text StackValueText;// Reference to the text attached to the icon showing how many stacks are currently on the player
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<CombatPlayer>();
        myClass = GetComponent<EnemyClass>();
    }

    protected virtual void OnDisable()
    {
        var ObjectToDestroy = StatusTray.EnemyEffects[EffectLabel];// Reference to the object that will be destroyed and removed from the dictionary
        StatusTray.EnemyEffects.Remove(EffectLabel);// Remove the gameobject attached to this key
        Destroy(ObjectToDestroy);// Destroy this gameobject
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
        StackValueText = StatusTray.EnemyEffects[this.EffectLabel].transform.Find("Stack Value").GetComponent<TMP_Text>();// Reference is set and will be used to manipulate the text
        UpdateStatusIcon();// Expose the variable as a string
    }
    private void Countdown()
    {
        turnCounter--;
        UpdateStatusIcon();// Expose the variable as a string
        if (turnCounter <= 0)
            Destroy(this);
    }
    public virtual void Effect(EnemyClass attackingEnemy, int Damage)
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
        UpdateStatusIcon();
    }
    protected virtual void UpdateStatusIcon()
    {
        StackValueText.text = $"{this.turnCounter}";// Expose the variable as a string
    }
}

