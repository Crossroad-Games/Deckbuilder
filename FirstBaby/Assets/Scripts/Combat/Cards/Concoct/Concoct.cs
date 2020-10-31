using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardPorpuse { Attack, Defense, Effect, Any }
public class Concoct : MonoBehaviour
{
    #region References
    private Card myCard;
    private CombatPlayer combatPlayer;
    private CombatManager combatManager;
    private EnemyClass targetEnemy;
    private GameObject concoctUI;
    #endregion
    public bool isConcocting = false; 
    public CardPorpuse concoctPorpuse;
    private List<Card> cardsToConcoct = new List<Card>();
    [SerializeField] Vector3 concoctCardPosition;



    private void Awake()
    {
        myCard = GetComponent<Card>();
        combatPlayer = FindObjectOfType<CombatPlayer>();
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
    }

    private void Start()
    {
        concoctUI = combatManager.concoctUI;
    }

    void Update()
    {
        if (isConcocting)
        {
            DoConcoct();
        }
    }

    public void DoConcoct()
    {
        transform.position = Vector3.Lerp(transform.position, concoctCardPosition, myCard.CombatProperties.cardDrawingSpeed * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            if (combatPlayer.hitInfo.collider != null && combatPlayer.isHoveringCard) //If mouse is over a card when it is pressed
            {
                Card card = combatPlayer.hitInfo.collider.gameObject.GetComponent<Card>();
                if (!card.concocted && card.cardPorpuse == concoctPorpuse)
                {
                    if (cardsToConcoct.Count == 0)
                    {
                        Debug.Log("cardConcocted");
                        cardsToConcoct.Add(card);
                        card.concocted = true;
                        Debug.Log(card.highlightPreviousHeight);
                    }
                    else
                    {
                        Debug.Log("há cartas no concoctlist");
                        if (card.GetType() == cardsToConcoct[0].GetType())
                        {
                            cardsToConcoct.Add(card);
                            card.concocted = true;
                            Debug.Log(card.highlightPreviousHeight);
                        }
                    }
                }
                else
                {
                    cardsToConcoct.Remove(card);
                    card.concocted = false;
                    Debug.Log(card.highlightPreviousHeight);
                }
            }

        }
    }

    public void ConfirmConcoct()
    {
        if (cardsToConcoct.Count > 0)
        {
            switch (concoctPorpuse)
            {
                case CardPorpuse.Attack:
                    ConcoctCardAttack myConcoctAttackCard = myCard as ConcoctCardAttack;
                    if (myConcoctAttackCard != null)
                    {
                        myConcoctAttackCard.BringConcoctInfo(cardsToConcoct);
                        FinishConcoct();
                        //TODO: Call animation here, and make an animation event that will call the DealDamage function
                        myConcoctAttackCard.DealDamage();
                    }
                    break;
            }
        }
        else
            Debug.Log("no card concocted");
    }

    public void CancelConcoct() //Called when canceled concoct
    {
        combatManager.confirmConcoctButton.onClick.RemoveAllListeners();
        combatManager.cancelConcoctButton.onClick.RemoveAllListeners();
        Debug.Log("deu unsubscribe");
        isConcocting = false;
        concoctUI.SetActive(false);
        myCard.selectable = true;
        myCard.followCardPositionToFollow = true; //Card restart following it's target in hand
        if (combatPlayer.OnCardUnselected != null)
        {
            combatPlayer.OnCardUnselected(myCard.gameObject);//Call CardUnselected event
            foreach (Card card in cardsToConcoct)
            {
                card.concocted = false;
                combatPlayer.OnCardUnselected(card.gameObject); // This is called because the concocted card is also highlighted, and the Unselected event calls unhighlight
            }
        }
        foreach (Card card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = true;
        }
        cardsToConcoct.Clear();
        switch (concoctPorpuse)
        {
            case CardPorpuse.Attack:
                ConcoctCardAttack myConcoctAttackCard = myCard as ConcoctCardAttack;
                if (myConcoctAttackCard != null)
                {
                    myConcoctAttackCard.canceledConcoct = true;
                }
                break;
                //TODO: ConcoctDefenseCard canceled = true
                //TODO: ConcoctEffectCard canceled = true
                //TODO: ConcoctHybridCard canceled = true
        }
    }

    public void StartConcoct()
    {
        combatManager.confirmConcoctButton.onClick.AddListener(ConfirmConcoct);
        combatManager.cancelConcoctButton.onClick.AddListener(CancelConcoct);
        Debug.Log("deu subscribe");
        myCard.followCardPositionToFollow = false; //Card stops following it's target in hand
        myCard.selectable = false;
        isConcocting = true; //Update isConcocting boolean
        foreach(Card card in myCard.playerHand.physicalCardsInHand)
        {
            if(card != myCard)
                card.selectable = false;
        }
        //Activate Concoct Confirm/Cancel UI
        concoctUI.SetActive(true);
    }

    public void StartConcoct(EnemyClass targetEnemy)
    {
        combatManager.confirmConcoctButton.onClick.AddListener(ConfirmConcoct);
        combatManager.cancelConcoctButton.onClick.AddListener(CancelConcoct);
        Debug.Log("deu subscribe");
        this.targetEnemy = targetEnemy;
        myCard.followCardPositionToFollow = false; //Card stops following it's target in hand
        myCard.selectable = false;
        isConcocting = true; //Update isConcocting boolean
        foreach (Card card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = false;
        }
        //Activate Concoct Confirm/Cancel UI
        concoctUI.SetActive(true);
    }

    public void FinishConcoct() //Called when confirmed concoct
    {
        combatManager.confirmConcoctButton.onClick.RemoveAllListeners();
        combatManager.cancelConcoctButton.onClick.RemoveAllListeners();
        Debug.Log("deu unsubscribe");
        isConcocting = false;
        concoctUI.SetActive(false);
        foreach(Card card in cardsToConcoct)
        {
            myCard.playerHand.SendCard(card.cardInfo, myCard.Player.CdPile);
        }
        foreach (Card card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = true;
        }
    }
}
