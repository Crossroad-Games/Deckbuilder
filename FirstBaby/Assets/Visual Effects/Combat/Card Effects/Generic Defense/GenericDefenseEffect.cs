using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GenericDefenseEffect : MonoBehaviour
{
    public PhysicalCard card;
    private VisualEffect visualEffect;
    private Animator anim;
    private bool actuated;

    private void Update()
    {
        
    }

    public void StopSpawn()
    {
        visualEffect.SetFloat("Rate", 0);
    }

    public void DealEffect()
    {
        if (card.cardPorpuse == CardPorpuse.Attack)
        {
            StartCoroutine(card.DealDamage());
        }
        else
        {
            StartCoroutine(card.GainShield_Health());
        }
        card.DealEffect();
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}
