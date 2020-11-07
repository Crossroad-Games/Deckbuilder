using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FalloutKeyword : VirtualCardExtension
{
    protected override void Awake()
    {
        base.Awake();
        Keyword = "Fallout";
    }

    public abstract void FalloutEffect();

    public override void ExtensionEffect() => FalloutEffect();// Execute its Overflow Effect
}
