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
        if (card.cardPorpuse == CardPorpuse.Attack)
        {
            throw new MissingReferenceException("This effect is defense only");
        }
        else
        {
            StartCoroutine(card.GainShield_Health());
            card.EndGainShield_Health();
            if (card.type == "ConcoctCardDefense")
            {
                ConcoctCardDefense concoctCard = card as ConcoctCardDefense;
                StartCoroutine(concoctCard.GainShield_Health(concoctCard.myConcoct.cardsToConcoct));
                concoctCard.EndGainShield_Health();
                Debug.Log("chamou dealDamage");
            }
        }
        if(dealEffect)
            card.DealEffect();
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}
