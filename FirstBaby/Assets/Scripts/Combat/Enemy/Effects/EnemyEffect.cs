using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public string EffectLabel;
    protected EnemyStatusTray StatusTray;
    protected virtual void Awake()
    {
        StatusTray = GetComponent<EnemyStatusTray>();// Gets the Status Tray Component reference
        StatusTray.OnAddEffect?.Invoke(this);
    }
}
