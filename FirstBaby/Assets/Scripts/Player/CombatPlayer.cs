using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CombatPlayer : MonoBehaviour
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField]private int PlayerHP; // Current HP
    [SerializeField]private int PlayerMaxHP;// Maximum HP
    [SerializeField]private int PlayerDefense;// Player Defense stat
    [SerializeField] private string Name;// Could be either a username or a preset name?
    #endregion
    [Space(5)]
    [SerializeField] private Deck deck;
    [SerializeField] private Hand hand;
    [SerializeField] private Button EndTurnButton;
    private TurnManager TurnMaster;

    // Start is called before the first frame update
    void Start()
    {
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseGame.IsPaused)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            deck.SendCard(deck.cardsList[0], hand); // This would be the draw, sending the first card of deck to hand
            Debug.Log("Chamou SendCard");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            deck.Shuffle(); //shuffles the Deck
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            hand.SendCard(hand.cardsList[0], deck);
        }
    }
    public void FlipEndButton(bool Interactable)
    {
        EndTurnButton.interactable = Interactable;// Toggle the button
    }
    public void EndTurn()
    {
        if (TurnManager.State==CombatState.PlayerActionPhase)
            TurnMaster.EndPlayerTurn();
    }
    public void ProcessDamage(int Damage)
    {
        Damage = Damage - PlayerDefense;// Reduce the damage by the enemy defense
        Damage = Damage <= 0 ? 0 : Damage;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(Damage);// Apply damage to the enemy's HP
    }
    private void LoseLife(int Amount) => PlayerHP -= Amount;

}
