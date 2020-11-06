using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverflowKeyword : VirtualCardExtension
{
    protected override void Awake()
    {
        base.Awake();
        Keyword = "Overflow";
    }
    public abstract void OverflowEffect();
    public override void ExtensionEffect() => OverflowEffect();// Execute its Overflow Effect
}
