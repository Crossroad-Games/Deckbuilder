using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatusTray : MonoBehaviour
{
    public Dictionary<string,GameObject> PlayerEffects;// List of effects currently on the player
    public Action<PlayerEffect> OnAddEffect;// Event called whenever a new status is added
    [SerializeField] private Vector3 FirstIcon = new Vector3(-.75f,-1f,0);// Position of the first icon
    [SerializeField] private float OffsetIcon = .35f;// Distance from one icon to another
    private void Awake()
    {
        OnAddEffect += ExpandTray;// Subscribe to the event
        PlayerEffects = new Dictionary<string, GameObject>();
    }
    public void ExpandTray(PlayerEffect newEffect)// Add a new icon to the status tray
    {
        Debug.Log("UI/StatusIcons/" + newEffect.EffectLabel);
        if (!PlayerEffects.ContainsKey(newEffect.EffectLabel))// If there is no instance of the applied effect
        {
            var IconSpawn = Instantiate(Resources.Load("UI/StatusIcons/" + newEffect.EffectLabel)) as GameObject;// Instantiates a new icon based on the effect's label
            IconSpawn.transform.SetParent(this.transform);// Set as a child of the player object
            IconSpawn.transform.localPosition = new Vector3(FirstIcon.x + PlayerEffects.Count * .35f, FirstIcon.y, FirstIcon.z);// Set the icon's position based on how many icons are currently on the scene
            PlayerEffects.Add(newEffect.EffectLabel, IconSpawn);// Add the icon to the dictionary under the key newEffect
        }
    }
}
