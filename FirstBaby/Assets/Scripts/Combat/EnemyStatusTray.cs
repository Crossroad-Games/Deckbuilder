using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class EnemyStatusTray : MonoBehaviour
{
    public Dictionary<string, GameObject> EnemyEffects;// List of effects currently on the Enemy
    public Action<EnemyEffect> OnAddEffect;// Event called whenever a new status is added
    [SerializeField] private Vector3 FirstIcon = new Vector3(-.75f, -1.7f, 0);// Position of the first icon
    [SerializeField] private float OffsetIcon = .35f;// Distance from one icon to another
    private void Awake()
    {
        OnAddEffect += ExpandTray;// Subscribe to the event
        EnemyEffects = new Dictionary<string, GameObject>();
    }
    public void ExpandTray(EnemyEffect newEffect)// Add a new icon to the status tray
    {
        Debug.Log("UI/StatusIcons/" + newEffect.EffectLabel);
        if (!EnemyEffects.ContainsKey(newEffect.EffectLabel))// If there is no instance of the applied effect
        {
            var IconSpawn = Instantiate(Resources.Load("UI/StatusIcons/" + newEffect.EffectLabel)) as GameObject;// Instantiates a new icon based on the effect's label
            IconSpawn.transform.SetParent(this.transform);// Set as a child of the Enemy object
            IconSpawn.transform.localPosition = new Vector3(FirstIcon.x + EnemyEffects.Count * .35f, FirstIcon.y, FirstIcon.z);// Set the icon's position based on how many icons are currently on the scene
            EnemyEffects.Add(newEffect.EffectLabel, IconSpawn);// Add the icon to the dictionary under the key newEffect
        }
    }
}
