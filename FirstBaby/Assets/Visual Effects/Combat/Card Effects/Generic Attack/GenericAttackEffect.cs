using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GenericAttackEffect : MonoBehaviour
{
    public PhysicalCard card;
    public Transform targetTransform;
    private Animator anim;
    private bool moveToTarget;
    private bool actuated;
    public bool dealEffect;
    public float orbMovementSpeed;
    private Vector3 UsingPosition= new Vector3(0,6f,0);// Position the card is held when executing its animation
    // Update is called once per frame
    void Start()
    {
        anim = GetComponent<Animator>();
        actuated = false;
        card.selectable = false;// No longer selectable
        card.followCardPositionToFollow = false;// No longer follows
        card.transform.localPosition = UsingPosition;// Fixates the card at this position
    }
    void Update()
    {
        if (moveToTarget)
        {
            if (targetTransform != null)
            {
                Debug.Log("Tem um target transform: " + targetTransform);
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, orbMovementSpeed * Time.deltaTime); // Move the orb to enemy position
                if ((transform.position - targetTransform.position).magnitude <= 0.2f && !actuated)
                {
                    actuated = true;
                    if (dealEffect)
                        card.DealEffect();
                    if (card.cardPorpuse == CardPorpuse.Attack)
                    {
                        if (card.type == "ConcoctCardAttack")
                        {
                            ConcoctCardAttack concoctCard = card as ConcoctCardAttack;
                            StartCoroutine(concoctCard.DealDamage(concoctCard.myConcoct.cardsToConcoct));
                            concoctCard.EndDealDamage();
                            Debug.Log("chamou dealDamage");
                        }
                        else
                        {
                            StartCoroutine(card.DealDamage());
                            card.EndDealDamage();
                        }
                    }
                    else
                    {
                        throw new MissingReferenceException("This effect is attack only");
                    }
                    anim.SetTrigger("ActivateExplosion");
                }
            }
            else
            {
                Debug.Log("Não tem target transform: "+targetTransform);
            }
        }
    }

    public void beginMovement()
    {
        Debug.Log("Começou a mover");
        moveToTarget = true;
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}
