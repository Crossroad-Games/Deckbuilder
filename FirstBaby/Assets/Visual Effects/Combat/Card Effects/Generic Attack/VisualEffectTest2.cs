using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectTest2 : MonoBehaviour
{
    public PhysicalCard card;
    public Transform targetTransform;
    private Animator anim;
    private bool moveToTarget;
    private bool actuated;
    public bool dealEffect;
    public float orbMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        actuated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveToTarget)
        {
            if(targetTransform != null)
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, orbMovementSpeed * Time.deltaTime); // Move the orb to enemy position
            if((transform.position - targetTransform.position).magnitude <= 0.2f && !actuated)
            {
                actuated = true;
                if(dealEffect)
                    card.DealEffect();
                if (card.cardPorpuse == CardPorpuse.Attack)
                {
                    StartCoroutine(card.DealDamage());
                    card.EndDealDamage();
                    if(card.type == "ConcoctCardAttack")
                    {
                        ConcoctCardAttack concoctCard = card as ConcoctCardAttack;
                        StartCoroutine(concoctCard.DealDamage(concoctCard.myConcoct.cardsToConcoct));
                        concoctCard.EndDealDamage();
                        Debug.Log("chamou dealDamage");
                    }
                }
                else
                {
                    throw new MissingReferenceException("This effect is attack only");
                }
                anim.SetTrigger("ActivateExplosion");
            }
        }
    }

    public void beginMovement()
    {
        moveToTarget = true;
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}
