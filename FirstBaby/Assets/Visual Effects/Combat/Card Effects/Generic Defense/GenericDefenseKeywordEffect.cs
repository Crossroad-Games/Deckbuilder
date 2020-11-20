using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GenericDefenseKeywordEffect : MonoBehaviour
{
    public VirtualCard virtualCard;
    private VisualEffect visualEffect;
    private Animator anim;
    public GameObject cardUI;
    private bool actuated;
    public bool dealEffect;


    private void Update()
    {

    }

    public void StopSpawn()
    {
        visualEffect.SetFloat("Rate", 0);
    }

    public void DealEffect()
    {
        foreach (KeyValuePair<string, VirtualCardExtension> extensionEffect in virtualCard.virtualCardExtensions)
        {
            extensionEffect.Value.DealEffect();
        }
        ByeByeCardUI();
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }

    public void ByeByeCardUI()
    {
        Destroy(cardUI);
    }
}
