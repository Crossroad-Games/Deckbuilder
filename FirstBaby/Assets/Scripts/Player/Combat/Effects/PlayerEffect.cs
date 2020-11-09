using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    protected PlayerStatusTray StatusTray;
    public string EffectLabel;
    protected virtual void Awake()
    {
        StatusTray = GetComponent<PlayerStatusTray>();// Gets the Status Tray Component reference
        StatusTray.OnAddEffect?.Invoke(this);
    }
}
