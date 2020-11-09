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

    public abstract void FalloutEffect();  //Fallout effect is called ont he SendCard method from drawPiles if it was called by a card (boolean parameter true)

    public override void ExtensionEffect() => FalloutEffect();// Execute its Overflow Effect
}
