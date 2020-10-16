using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RewardManager : MonoBehaviour
{
    private CardDatabase cardDatabase;
    [Header("UI elements")]
    [SerializeField] private GameObject CardSelectionUI = null;
    [SerializeField] private List<Button> cardOptionsButtons = null;
    [SerializeField] private List<TMP_Text> cardOptionsCosts = null;

    #region References
    private Deck playerDeck;// Reference to the player's deck
    private CombatPlayer combatPlayer;// Reference to the combat player
    private DungeonPlayer dungeonPlayer; //Reference to the player in the dungeon
    #endregion

    private Dictionary<Button, CardInfo> cardOptions = new Dictionary<Button, CardInfo>();


    private void Awake()
    {
        //Initialization
        cardDatabase = GameObject.Find("Card Database").GetComponent<CardDatabase>();
        //--------------------
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Card Selectiion
    public void FillCombatCardSelection()
    {
        combatPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        ClearCardOptions();
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        
        for(int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;
            cardOptionsCosts[i].text = "Cost: " + cardInfoInstance.ResourceCost;
            if (combatPlayer.myData.PlayerLifeForce > cardInfoInstance.ResourceCost)
                cardOptionsCosts[i].color = Color.green;
            else
                cardOptionsCosts[i].color = Color.red;
            databaseCopy.RemoveAt(r);
        }
    }

    public void FillDungeonCardSelection()
    {
        dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        ClearCardOptions();
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        for (int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;
            cardOptionsCosts[i].text = "Cost: " + cardInfoInstance.ResourceCost;
            if (dungeonPlayer.myData.PlayerLifeForce > cardInfoInstance.ResourceCost)
                cardOptionsCosts[i].color = Color.green;
            else
                cardOptionsCosts[i].color = Color.red;
            databaseCopy.RemoveAt(r);
        }
    }

    public void ClearCardOptions()
    {
        cardOptions.Clear();
        foreach(Button cardOptionButton in cardOptionsButtons)
        {
            cardOptionButton.image.sprite = null;
            cardOptions.Clear();
        }
    }

    #region Combat Reward
    public void Combat_ChooseCard0()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[0]] != null)
        {
            if (combatPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[0]].ResourceCost)
            {
                playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[0]]);// Adds card to player deck, *temporary*  TODO: Add card to player's owned card
                combatPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[0]].ResourceCost;
                cardOptions[cardOptionsButtons[0]] = null;
                StopSelection();
            }
            else
            {
                Debug.Log("Not enought resource/HP");
            }
        }
        else
            throw new Exception("there is no cardInfo attached to this button");
    }
    public void Combat_ChooseCard1()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[1]] != null)
        {
            if (combatPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[1]].ResourceCost)
            {
                playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[1]]);// Adds card to player deck, *temporary*  TODO: Add card to player's owned card
                combatPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[1]].ResourceCost;
                cardOptions[cardOptionsButtons[1]] = null;
                StopSelection();
            }
        }
        else
            throw new Exception("there is no cardInfo attached to this button");
    }
    public void Combat_ChooseCard2()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[2]] != null)
        {
            if (combatPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[2]].ResourceCost)
            {
                playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[2]]);// Adds card to player deck, *temporary*  TODO: Add card to player's owned card
                combatPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[2]].ResourceCost;
                cardOptions[cardOptionsButtons[2]] = null;
                StopSelection();
            }
        }
        else
            throw new Exception("there is no cardInfo attached to this button");
    }
    #endregion

    #region Dungeon Reward
    public void Dungeon_ChooseCard0()
    {
        dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        if(cardOptions[cardOptionsButtons[0]] != null)
        {
            if(dungeonPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[0]].ResourceCost)
            {
                dungeonPlayer.myData.CardCollectionID.Add(cardOptions[cardOptionsButtons[0]].ID);
                dungeonPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[0]].ResourceCost;
                cardOptions[cardOptionsButtons[0]] = null;
                //TODO: Stop Selection
                dungeonPlayer.GetComponent<PlayerMovement>().canMove = true;
            }
        }
    }
    public void Dungeon_ChooseCard1()
    {
        dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        if (cardOptions[cardOptionsButtons[1]] != null)
        {
            if (dungeonPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[1]].ResourceCost)
            {
                dungeonPlayer.myData.CardCollectionID.Add(cardOptions[cardOptionsButtons[1]].ID);
                dungeonPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[1]].ResourceCost;
                cardOptions[cardOptionsButtons[1]] = null;
                //TODO: Stop Selection
                dungeonPlayer.GetComponent<PlayerMovement>().canMove = true;
            }
        }
    }
    public void Dungeon_ChooseCard2()
    {
        dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
        if (cardOptions[cardOptionsButtons[2]] != null)
        {
            if (dungeonPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[2]].ResourceCost)
            {
                dungeonPlayer.myData.CardCollectionID.Add(cardOptions[cardOptionsButtons[2]].ID);
                dungeonPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[2]].ResourceCost;
                cardOptions[cardOptionsButtons[2]] = null;
                //TODO: Stop Selection
                dungeonPlayer.GetComponent<PlayerMovement>().canMove = true;
            }
        }
    }
    #endregion

    public void StopSelection()
    {
        var dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName.Dungeon");// Saves the information at this location
        CardSelectionUI.SetActive(false);// Deactivate the card selection UI
        DungeonGameData.Current.PlayerData.PlayerLifeForce = playerDeck.gameObject.GetComponent<CombatPlayer>().myData.PlayerLifeForce;// Updates the dungeon Life Force to be the same as the combat
        string jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        SceneManager.LoadSceneAsync(DungeonGameData.Current.DungeonScene, LoadSceneMode.Single);// Loads the Dungeon Scene
    }
    #endregion

}
