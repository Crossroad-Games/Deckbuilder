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
    private Vector3 UsingPosition= new Vector3(0,6,0);// Position the card is held when executing its animation
    private void Start()
    {
        card.selectable = false;// No longer selectable
        card.followCardPositionToFollow = false;// No longer follows
        card.transform.localPosition = UsingPosition;// Fixates the card at this position
    }
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
            if (card.type == "ConcoctCardDefense")
            {
                ConcoctCardDefense concoctCard = card as ConcoctCardDefense;
                StartCoroutine(concoctCard.GainShield_Health(concoctCard.myConcoct.cardsToConcoct));
                concoctCard.EndGainShield_Health();
                Debug.Log("chamou dealDamage");
            }
            else
            {
                StartCoroutine(card.GainShield_Health());
                card.EndGainShield_Health();
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
