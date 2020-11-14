using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectTest : MonoBehaviour
{
    public AlchemistFireCard card;
    public VisualEffect visualEffectAsset;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    public void DealEffect()
    {
        //card.DealEffect();
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}
