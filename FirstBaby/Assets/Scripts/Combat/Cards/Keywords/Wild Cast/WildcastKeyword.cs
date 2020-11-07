using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WildcastKeyword : VirtualCardExtension
{
    protected override void Awake()
    {
        base.Awake();
        Keyword = "Wildcast";
    }
    public override void ExtensionEffect() => WildCastEffect();// Executes this action
    public abstract void WildCastEffect();// Each type of wildcast will have its own definition
}
